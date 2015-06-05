using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void test1() {
            ExpressionValidatorLib.ExprValidator psr = new ExprValidator();
            int total_tests = 0;
            int passed_tests = 0;

            //
            // Tests that should result in pass.
            //
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

            //
            // Tests that should result in fail.
            //
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

            //
            // Show result.
            //
            double passRate = passed_tests * 1.0 / total_tests;
            string strPassRate = (passRate * 100).ToString("#.##") +"%";

            System.Console.WriteLine("");
            System.Console.WriteLine("total tests: " + total_tests);
            System.Console.WriteLine("passed tests: " + passed_tests);
            System.Console.WriteLine("pass rate: " + strPassRate);
        }


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
