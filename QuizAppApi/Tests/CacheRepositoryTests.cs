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

        [SetUp]
        public void Setup()
        {
            _cacheRepository = new CacheRepository();
        }

        [Test]
        public void Add_ThrowsTypeMismatchException()
        {
            _cacheRepository.Add("key", 42);

            Assert.Throws<TypeMismatchException>(() => _cacheRepository.Add("key", "string"));
        }

        [Test]
        public void Add_DoesNotThrowException()
        {
            _cacheRepository.Add("key", "firstString");

            Assert.DoesNotThrow(() => _cacheRepository.Add("key", "secondString"));
        }

        [Test]
        public void Retrieve_ThrowsTypeMismatchException()
        {
            _cacheRepository.Add("key", 42);

            Assert.Throws<TypeMismatchException>(() => _cacheRepository.Retrieve<string>("key"));
        }

        [Test]
        public void Retrieve_DoesNotThrowException()
        {
            _cacheRepository.Add("key", "stringValue");

            Assert.DoesNotThrow(() => _cacheRepository.Retrieve<string>("key"));
        }

        [Test]
        public void Clear_RemovesKey()
        {
            _cacheRepository.Add("key", "value");

            _cacheRepository.Clear("key");

            Assert.IsEmpty(_cacheRepository.Retrieve<string>("key"));
        }
    }
}
