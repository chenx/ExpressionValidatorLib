using System;
using ExpressionValidatorLib;

namespace Tester
{
    class Test
    {
        private bool verbose = true;

        static void Main(string[] args)
        {
            Test t = new Test();
            t.test1();

            Console.ReadLine();
        }

        /// <summary>
        /// Test function 1.
        /// </summary>
        private void test1() {
            ExpressionValidatorLib.ExprValidator psr = new ExprValidator();
            int total_tests = 0;
            int passed_tests = 0;

            test1_good(ref psr, ref total_tests, ref passed_tests);
            test1_bad(ref psr, ref total_tests, ref passed_tests);

            showTestResult(total_tests, passed_tests);
        }

        /// <summary>
        /// Show test result: total count, passed count, pass rate.
        /// </summary>
        /// <param name="total_tests"></param>
        /// <param name="passed_tests"></param>
        private void showTestResult(int total_tests, int passed_tests) {
            double passRate = passed_tests * 1.0 / total_tests;
            string strPassRate = (passRate * 100).ToString("#.##") + "%";

            System.Console.WriteLine("");
            System.Console.WriteLine("total tests: " + total_tests);
            System.Console.WriteLine("passed tests: " + passed_tests);
            System.Console.WriteLine("pass rate: " + strPassRate);
        }
        
        /// <summary>
        /// Tests that should result in pass.
        /// </summary>
        private void test1_good(ref ExpressionValidatorLib.ExprValidator psr, ref int total_tests, ref int passed_tests) {
            string[] good_tests = new string[] {
                "a",
                "2",
                "a + 2",
                "(2)",
                "(a)",
                "(2 + 1)",
                "(2 + 3 * 1 / 5)",
            };

            foreach (string test in good_tests)
            {
                assert(test, psr.Validate(test), true, psr.Message(), ref total_tests, ref passed_tests);
            }
        }

        /// <summary>
        /// Tests that should result in fail.
        /// </summary>
        private void test1_bad(ref ExpressionValidatorLib.ExprValidator psr, ref int total_tests, ref int passed_tests)
        {
            string[] bad_tests = new string[] {
                "a.",
                "2.",
                "a  2",
                " + 3",
                "(2",
                "a)",
                "2 + 1)",
                "(2 + - 3 * 1 / 5)",
            };

            foreach (string test in bad_tests)
            {
                assert(test, psr.Validate(test), false, psr.Message(), ref total_tests, ref passed_tests);
            }
        }

        /// <summary>
        /// Assert function.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="result"></param>
        /// <param name="expected"></param>
        /// <param name="Message"></param>
        /// <param name="total_tests"></param>
        /// <param name="passed_tests"></param>
        private void assert(string test, bool result, bool expected, string Message, ref int total_tests, ref int passed_tests)
        {
            ++total_tests;
            bool ok = (result == expected);
            passed_tests += ok ? 1 : 0;

            System.Console.Write("(" + total_tests + "): " + test + " ... expected: " + expected + " ... " + (ok ? "ok" : "failed"));
            if (verbose && Message != "") { System.Console.Write(Message + "\n"); }
            System.Console.WriteLine("");
        }
    }
}
