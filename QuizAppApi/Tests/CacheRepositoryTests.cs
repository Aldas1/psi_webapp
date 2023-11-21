using NUnit.Framework;
using QuizAppApi.Exceptions;
using QuizAppApi.Interfaces;
using QuizAppApi.Repositories;
using System;

namespace QuizAppApi.Tests.Repositories
{
    [TestFixture]
    public class CacheRepositoryTests
    {
        private ICacheRepository _cacheRepository;

        [Test]
        public void AddWrongType_ThrowsTypeMismatch()
        {
            _cacheRepository = new CacheRepository();
            _cacheRepository.Add("key", 42);

            Assert.Throws<TypeMismatchException>(() => _cacheRepository.Add("key", "string"));
        }

        [Test]
        public void AddSameType_StoresItems()
        {
            _cacheRepository = new CacheRepository();
            string stringValue = "firstString";
            _cacheRepository.Add("key", stringValue);

            Assert.AreEqual(stringValue, _cacheRepository.Retrieve<string>("key").FirstOrDefault());
        }

        [Test]
        public void RetrieveWrongType_ThrowsTypeMismatch()
        {
            _cacheRepository = new CacheRepository();
            _cacheRepository.Add("key", 42);

            Assert.Throws<TypeMismatchException>(() => _cacheRepository.Retrieve<string>("key"));
        }

        [Test]
        public void Clear_RemovesItems()
        {
            _cacheRepository = new CacheRepository();
            _cacheRepository.Add("key", "value");
            _cacheRepository.Clear("key");

            Assert.IsEmpty(_cacheRepository.Retrieve<string>("key"));
        }
    }
}
