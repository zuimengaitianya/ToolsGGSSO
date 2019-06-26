using Moq;
using System;
using Xunit;

namespace ToolsGG.XUnitTest
{
    public class UnitTest1 : IDisposable
    {
        [Fact]
        public void Add()
        {
            IAdds iadd = new Adds();
            Arithmetic arithmetic = new Arithmetic(iadd);
            int res = arithmetic.Add(1, 2);
            Assert.Equal(3, res);
        }

        [Fact]
        public void StubAdd()
        {
            IAdds iadd = new StubAdds();
            Arithmetic arithmetic = new Arithmetic(iadd);
            int res = arithmetic.Add(1, 2);
            Assert.Equal(1, res);
        }

        [Fact]
        public void MockAdd()
        {
            var mockIAdd = new Mock<IAdds>();
            mockIAdd.Setup(m => m.Add(1, 2)).Returns(2);
            Arithmetic arithmetic = new Arithmetic(mockIAdd.Object);
            int res = arithmetic.Add(1, 2);
            Assert.Equal(2, res);
            res = arithmetic.Add(1, 4);//
            Assert.Equal(0, res);
        }

        [Fact]
        public void MockAnyAdd()
        {
            var mockIAdd = new Mock<IAdds>();
            mockIAdd.Setup(m => m.Add(It.IsAny<int>(), It.IsAny<int>())).Returns(4);
            Arithmetic arithmetic = new Arithmetic(mockIAdd.Object);
            int res = arithmetic.Add(1, 2);
            Assert.Equal(4, res);
        }

        [Fact]
        public void Divide()
        {
            Arithmetic arithmetic = new Arithmetic();
            Assert.Throws<DivideByZeroException>(() => arithmetic.Divide(1, 0));
        }


        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 0)]
        [InlineData(0, 2)]
        public void AddMuItiple(int parmaA, int parmaB)
        {
            IAdds iadd = new Adds();
            Arithmetic arithmetic = new Arithmetic(iadd);
            int res = arithmetic.Add(parmaA, parmaB);
            Assert.Equal(parmaA + parmaB, res);
        }


        public void Dispose()
        {
            Console.WriteLine("测试完成清空数据");
        }

    }

    public interface IAdds
    {
        int Add(int nb1, int nb2);
    }

    public class Adds : IAdds
    {
        public int Add(int nb1, int nb2)
        {
            return nb1 + nb2;
        }
    }

    public class StubAdds:IAdds
    {
        public int Add(int nb1, int nb2)
        {
            return 1;
        }

    }

    public class Arithmetic
    {
        IAdds iAdd;

        public Arithmetic()
        {

        }

        public Arithmetic(IAdds add)
        {
            this.iAdd = add;
        }

        public int Add(int nb1, int nb2)
        {
            return iAdd.Add(nb1, nb2);
        }

        public int Divide(int nb1, int nb2)
        {
            return nb1 / nb2;
        }

    }

}
