using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Tasks;

namespace Tasks.Tests
{
    [TestClass]
    public class TradeDayProcessorTest
    {
        [TestMethod]
        public void Constructor_Assigns_ProvidedValues_ToPrivateFields()
        {
            Predicate<TradeDay> predicate = d => true;
            var processor = new TradeDayProcessor(3, "mydata.csv", predicate);

            var t = typeof(TradeDayProcessor);
            var numConsumersField = t.GetField("numConsumers", BindingFlags.NonPublic | BindingFlags.Instance);
            var tradeFileField = t.GetField("tradeFile", BindingFlags.NonPublic | BindingFlags.Instance);
            var testField = t.GetField("test", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(numConsumersField);
            Assert.IsNotNull(tradeFileField);
            Assert.IsNotNull(testField);

            Assert.AreEqual(3, (int)numConsumersField.GetValue(processor));
            Assert.AreEqual("mydata.csv", (string)tradeFileField.GetValue(processor));
            Assert.AreSame(predicate, testField.GetValue(processor));
        }

        [TestMethod]
        public void Constructor_Allows_Null_Predicate()
        {
            var processor = new TradeDayProcessor(1, "file.csv", null);
            var t = typeof(TradeDayProcessor);
            var testField = t.GetField("test", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNull(testField.GetValue(processor));
        }
    }
}
