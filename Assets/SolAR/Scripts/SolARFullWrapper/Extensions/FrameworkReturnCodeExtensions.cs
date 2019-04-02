using UnityEngine.Assertions;

namespace SolAR.Core
{
    public static class FrameworkReturnCodeExtensions
    {
        public static FrameworkReturnCode Check(this FrameworkReturnCode code)
        {
            Assert.AreEqual(FrameworkReturnCode._SUCCESS, code);
            return code;
        }
    }
}
