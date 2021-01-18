using Redstone.Common.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Redstone.Common.Tests.Collections
{
    public class CompactedLongArrayTest
    {
        [Fact]
        public void Test()
        {
            var array = new CompactedLongArray(4, 4096);

            array[0] = 3;
            array[1] = 42;

            var v = array[0];
            var v2 = array[1];
        }
    }
}
