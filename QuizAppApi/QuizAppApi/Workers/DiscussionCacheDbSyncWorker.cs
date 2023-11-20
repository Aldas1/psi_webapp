using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Exceptions;

namespace QuizAppApi.Workers;

public class DiscussionCacheDbSyncWorker
{
    private static readonly int _numberOfFetchedDays = 10;
    private readonly IHostApplicationLifetime _appLifetime;
    private static IServiceProvider _serviceProvider;
    private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

    private readonly Thread _worker = new Thread(() => DoWork(_cts.Token));

    private static void DoWork(CancellationToken cancellationToken)
    {
        var timeout = 10000;
        while (true)
        {
            Thread.Sleep(timeout);
            if (cancellationToken.IsCancellationRequested) break;
            Upload();
        }

        Upload();
    }

    private static void Upload()
    {
        using var scope = _serviceProvider.CreateScope();
        var cacheRepo = _serviceProvider.GetService<ICacheRepository>();
        var commentRepo = _serviceProvider.GetService<ICommentRepository>();
        if (cacheRepo == null) return;
        Monitor.Enter(cacheRepo.Lock);
        try
        {
            if (commentRepo == null) return;
            var comments = cacheRepo.Retrieve<Comment>("comments");
            foreach (var comment in comments)
            {
                if (comment.Stored) continue;
                commentRepo.AddComment(comment);
                comment.Stored = true;
            }
        }
        catch (TypeMismatchException)
        {
            cacheRepo.Clear("comments");
            var comments = cacheRepo.Retrieve<Comment>("comments");
            foreach (var comment in comments)
            {
                if (comment.Stored) continue;
                commentRepo.AddComment(comment);
                comment.Stored = true;
            }
        }
        finally
        {
            Monitor.Exit(cacheRepo.Lock);
        }
    }

    public DiscussionCacheDbSyncWorker(IHostApplicationLifetime appLifetime, IServiceProvider serviceProvider)
    {
        _appLifetime = appLifetime;
        _serviceProvider = serviceProvider;
    }

    private void FetchComments()
    {
        using var scope = _serviceProvider.CreateScope();
        var cacheRepo = _serviceProvider.GetService<ICacheRepository>();
        var commentRepo = _serviceProvider.GetService<ICommentRepository>();
        if (cacheRepo == null || commentRepo == null) return;
        Monitor.Enter(cacheRepo.Lock);
        try
        {
            var comments = commentRepo.GetByCondition(c => c.Date >= DateTime.Now.AddDays(-_numberOfFetchedDays));
            cacheRepo.Clear("comments");
            foreach (var comment in comments)
            {
                comment.Stored = true;
                cacheRepo.Add("comments", comment);
            }
        }
        finally
        {
            Monitor.Exit(cacheRepo.Lock);
        }
    }

    public void Start()
    {
        FetchComments();
        _appLifetime.ApplicationStopping.Register(async () =>
        {
            Upload();
            _cts.Cancel();
            _worker.Interrupt();
        });
        _worker.Start();
    }
}