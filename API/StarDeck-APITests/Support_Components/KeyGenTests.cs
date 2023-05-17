using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarDeck_API.Support_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StarDeck_API.Support_Components.Tests
{
    [TestClass()]
    public class KeyGenTests
    {
        [TestMethod()]
        public void CreatePatternTest()
        {
            KeyGen keyGen = KeyGen.GetInstance();
            string pattern = keyGen.CreatePattern("C-");
            Assert.IsTrue(Regex.IsMatch(pattern, @"C-[a-zA-Z0-9]{12}"));
        }
    }
}