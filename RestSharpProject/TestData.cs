using NUnit.Framework;
using System.Collections.Generic;

namespace RestSharpProject
{
    public class TestData
    {
        public static IEnumerable<TestCaseData> AddressGetTestData
        {
            get
            {
                yield return new TestCaseData(1, "4649 Peachwillow", "20-653", "Test", "Test").SetName("Id 1");
                yield return new TestCaseData(2, "Swieta 112321", "10-653", "Wroclaw", "Poland").SetName("Id 2");
            }
        }
        
        public static IEnumerable<TestCaseData> AddressPostTestData
        {
           get
            {
                yield return new TestCaseData("4649 Peachwillow", "20-653", "Wro", "Poland");
            }
        }

        public static IEnumerable<TestCaseData> AddressUpdateTestData
        {
            get
            {
                yield return new TestCaseData("4649 Peachwillow", "20-653", "Wro", "Poland", "31 Geschichte", "29-789", "Berlin", "Germany");
            }
        }
    }
}
