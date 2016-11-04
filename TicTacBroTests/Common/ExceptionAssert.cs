using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Qualtrax.Tests.Common
{
    public static class ExceptionAssert
    {
        public static T Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                return ex;
            }
            catch (Exception ex)
            {
                Assert.Fail(String.Format("Threw unexpected exception of type '{0}'", ex.GetType().Name));
            }

            Assert.Fail(String.Format("Did not throw expected exception of type {0}", typeof(T).Name));
            return null;
        }

        public static Exception Throws(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                return ex;
            }

            Assert.Fail("Did not throw an exception.");
            return null;
        }
    }
}