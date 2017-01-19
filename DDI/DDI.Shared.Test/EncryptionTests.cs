using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test
{
    [TestClass]
    public class EncryptionTests
    {
        private const string TESTDESCR = "Data | Encryption";
        [TestMethod, TestCategory(TESTDESCR)]
        public void Encrypt_ReturnsEncryptedText()
        {
            string encryptedText = Encryption.Encrypt("Password");

            Assert.IsTrue(encryptedText == "0N2ZjfhRuhJAuIAud0L/Jg==");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void Decrypt_ReturnsPlainText()
        {
            string decryptedText = Encryption.Decrypt("0N2ZjfhRuhJAuIAud0L/Jg==");

            Assert.IsTrue(decryptedText == "Password");
        }
    }
}
