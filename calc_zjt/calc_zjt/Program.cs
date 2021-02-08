using System;
using System.Collections.Generic;
using System.Linq;

namespace calc_zjt
{
    class Program
    {
        static List<string> Right = new List<string>();
        static List<string> Left = new List<string>();
        static string variable { get; set; }
        //check operator
        public static bool IsOperator(string str)
        {
            var opList = new List<string> { "+", "-", "*", "/", "(", ")" };
            if (opList.Contains(str))
                return true;
            return false;
        }
        //check element is digit 
        public static bool IsDigits(string str, int result = 0)
        {
            bool isNumb = int.TryParse(str, out result);
            return isNumb;
        }

        //Determine if the variable is valid
        public static bool Isvarible(char[] varible)
        {
            List<string> varib = new List<string>();
            for (int i = 0; i < varible.Length; i++)
            {
                char a = varible[i];
                if ((a >= 'a' && a <= 'z') || (a >= 'A' && a <= 'Z'))
                {
                    varib.Add(a.ToString());
                }
            }
            int n1 = 0;
            for (int i = 0; i < varib.Count; i++)
            {
                if (varib[0] == varib[i])
                {
                    n1 += 1;
                }
            }
            if (n1 == 0)
            {
                Console.WriteLine("varible worng");
                Environment.Exit(-1);
            }
            if (n1 == varib.Count)
            {
                variable = varib[0];
                return true;
            }
            else
            {
                Console.WriteLine("varible worng");
                Environment.Exit(-1);
                return false;
            }
        }
        //check formula ,reduce "------" "++++++"
        public static List<string> checkfrom(List<string> form)
        {
            for (int i = 0; i < form.Count; i++)
            {
                if (form[i] == "(")
                {
                    if (IsDigits(form[i - 1]))
                    {
                        form.Insert(i, "*");
                    }
                }
                if (IsOperator(form[i]) && form[i] != "(" && form[i] != ")")
                {
                    string operater = form[i];
                    if (form[i + 1] == operater)
                    {
                        form.Remove(form[i + 1]);
                        i = i - 1;
                    }
                }
            }
            return form;
        }

        //calc input list. after reduce opration return result;
        public static string operationList(List<string> x)
        {
            while (x.Count >= 0)
            {
                if (x[0] == "-" || x[0] == "+")
                {
                    x[1] = x[0] + x[1];
                    x.Remove(x[0]);
                }
                else if (x.Count == 1 || x.Count == 2)
                {
                    return x[0];
                }
                else
                {
                    x = Operation(x.Count, x);
                }
            }
            return x[0];
        }

        //remove Parenthesis
        public static void removeParenthesis(List<string> list)
        {
            int leftIndex = list.IndexOf("(");
            int rightIndex = list.IndexOf(")");
            int count = rightIndex - leftIndex + 1;
            string temp = "";
            string insideNumber = "";
            var Parenthesisinclude = list.GetRange(leftIndex + 1, rightIndex - leftIndex - 1);
            var after = new List<string>();

            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                temp += list[i];
            }

            if (leftIndex == 0 && rightIndex == list.Count - 1)
            {
                list.Remove("(");
                list.Remove(")");
                return;
            }

            if (!temp.Contains("x"))
            {
                insideNumber = operationList(Parenthesisinclude);
                if (leftIndex > 0)
                {
                    if (IsDigits(list[leftIndex - 1]))
                    {
                        list.RemoveRange(leftIndex, count);
                        list.Insert(leftIndex, "*");
                        list.Insert(leftIndex + 1, insideNumber);
                    }
                    else
                    {
                        list.RemoveRange(leftIndex, count);
                        list.Insert(leftIndex, insideNumber);
                    }
                }
            }
            else if (temp.Contains("x"))
            {
                string panrenthsisPro = "";

                for (int i = 0; i < leftIndex; i++)
                {
                    panrenthsisPro += list[i];
                }
                if (list.Last() != ")" && list[list.IndexOf(")") + 1] == "/")
                {
                    if (leftIndex > 1 && list[rightIndex - 1] == "-")
                    {
                        var eachplus = list.GetRange(rightIndex + 1, 2);
                        for (int i = Parenthesisinclude.Count - 1; i > 0; i--)
                        {
                            if (Parenthesisinclude[i] == "+")
                            {
                                Parenthesisinclude[i] = "-";
                            }
                            else if (Parenthesisinclude[i] == "-")
                            {
                                Parenthesisinclude[i] = "+";
                            }
                            if (Parenthesisinclude[i] == "+" || Parenthesisinclude[i] == "-")
                            {
                                Parenthesisinclude.InsertRange(i, eachplus);
                            }
                            list.RemoveRange(leftIndex, rightIndex - leftIndex + 1);
                            list.InsertRange(leftIndex, Parenthesisinclude);
                            return;
                        }
                    }
                    else
                    {
                        var eachplus = list.GetRange(rightIndex + 1, 2);
                        for (var i = Parenthesisinclude.Count - 1; i >= 0; i--)
                        {
                            if (Parenthesisinclude[i] == "+" || Parenthesisinclude[i] == "-")
                                Parenthesisinclude.InsertRange(i, eachplus);
                        }
                        list.RemoveRange(leftIndex, rightIndex - leftIndex + 1);
                        list.InsertRange(leftIndex, Parenthesisinclude);
                        return;
                    }
                }
                if (leftIndex > 0)
                {
                    if (list[list.IndexOf("(") - 1] == "+")
                    {
                        list.Remove("(");
                        list.Remove(")");
                    }
                    else if (list[leftIndex - 1] == "-")
                    {
                        Parenthesisinclude.Insert(0, "+");
                        for (var i = 0; i < Parenthesisinclude.Count; i++)
                        {
                            if (Parenthesisinclude[i] == "+")
                                Parenthesisinclude[i] = "-";
                            else if (Parenthesisinclude[i] == "-")
                                Parenthesisinclude[i] = "+";
                        }
                        list.RemoveRange(leftIndex - 1, rightIndex - leftIndex + 2);
                        list.InsertRange(leftIndex - 1, Parenthesisinclude);
                    }
                    else if (list[leftIndex - 1] == "*" || IsDigits(list[leftIndex - 1]))
                    {
                        if (leftIndex > 2 && list[leftIndex - 3] == "-")
                        {
                            var eachplus = list.GetRange(leftIndex - 2, 2);
                            for (var i = 0; i < Parenthesisinclude.Count; i++)
                            {
                                if (Parenthesisinclude[i] == "+")
                                    Parenthesisinclude[i] = "-";
                                else if (Parenthesisinclude[i] == "-")
                                    Parenthesisinclude[i] = "+";

                                if (Parenthesisinclude[i] == "+" || Parenthesisinclude[i] == "-")
                                    Parenthesisinclude.InsertRange(i + 1, eachplus);
                            }
                            list.RemoveRange(leftIndex, rightIndex - leftIndex + 1);
                            list.InsertRange(leftIndex, Parenthesisinclude);
                        }
                        else
                        {
                            var eachplus = list.GetRange(leftIndex - 2, 2);
                            for (var i = 0; i < Parenthesisinclude.Count; i++)
                            {
                                if (Parenthesisinclude[i] == "+" || Parenthesisinclude[i] == "-")
                                    Parenthesisinclude.InsertRange(i + 1, eachplus);
                            }
                            list.RemoveRange(leftIndex, rightIndex - leftIndex + 1);
                            list.InsertRange(leftIndex, Parenthesisinclude);
                        }
                    }
                }
            }
        }

        //move right X to Left
        public static void moveXToLeft(List<string> left, List<string> right)
        {
            List<string> temp = new List<string>();
            List<string> numList = new List<string>();

            for (var i = right.Count - 1; i >= 0; i--)
            {
                if (right[i] == "+" || right[i] == "-")
                {
                    temp = right.GetRange(i, right.Count - i);
                    string tempstring = "";
                    for (int j = 0; j < temp.Count; j++)
                    {
                        tempstring += temp[j];
                    }

                    if (tempstring.Contains("x"))
                    {
                        tempstring = "";
                        if (temp[0] == "+")
                            temp[0] = "-";
                        else if (temp[0] == "-")
                            temp[0] = "+";
                        left.AddRange(temp);
                    }
                    else
                    {
                        numList.AddRange(temp);
                    }
                    right.RemoveRange(i, right.Count - i);
                    temp.RemoveRange(0, temp.Count);
                }
            }

            string rightstring = "";
            for (int j = 0; j < right.Count; j++)
            {
                rightstring += right[j];
            }
            if (right.Count > 0 && rightstring.Contains("x"))
            {
                left.Add("-");
                left.AddRange(right);
                right.RemoveRange(0, right.Count);
            }
            right.AddRange(numList);
        }

        // move left number to right
        public static void moveDToRight(List<string> left, List<string> right)
        {
            List<string> temp = new List<string>();
            List<string> numList = new List<string>();

            for (var i = left.Count - 1; i >= 0; i--)
            {
                if (left[i] == "+" || left[i] == "-")
                {
                    temp = left.GetRange(i, left.Count - i);
                    string tempstring = "";
                    for (int j = 0; j < temp.Count; j++)
                    {
                        tempstring += temp[j];
                    }

                    if (!tempstring.Contains("x"))
                    {
                        tempstring = "";
                        if (temp[0] == "+")
                            temp[0] = "-";
                        else if (temp[0] == "-")
                            temp[0] = "+";
                        right.AddRange(temp);
                    }
                    else
                    {
                        numList.AddRange(temp);
                    }
                    left.RemoveRange(i, left.Count - i);
                    temp.RemoveRange(0, temp.Count);
                }
            }

            string lefttstring = "";

            for (int j = 0; j < left.Count; j++)
            {
                lefttstring += left[j];
            }
            if (right.Count > 0 && !lefttstring.Contains("x"))
            {
                left.Insert(0, "-");
                right.AddRange(left);
                left.RemoveRange(0, left.Count);
            }
            left.AddRange(numList);
        }

        // func about + _ * / which first to calc
        public static List<string> Operation(decimal z, List<string> question)
        {
            string calc = "";
            List<string> x = question;

            if (question.Contains("*") || question.Contains("/"))
            {
                if (question.Contains("*"))
                {
                    int x1 = x.IndexOf("*");
                    calc = multiply(x1, x);
                    x.RemoveRange(x1 - 1, 3);
                    x.Insert(x1 - 1, calc);
                    z = x.Count;
                }
                else
                {
                    int x1 = x.IndexOf("/");
                    calc = divide(x1, x);
                    x.RemoveRange(x1 - 1, 3);
                    x.Insert(x1 - 1, calc);
                    z = x.Count;
                }
            }
            else if (question.Contains("+") || question.Contains("-"))
            {
                if (question.Contains("+") && question.Contains("-"))
                {
                    if (question.IndexOf("+") < question.IndexOf("-"))
                    {
                        int x1 = x.IndexOf("+");
                        calc = plus(x1, x);
                        x.RemoveRange(x1 - 1, 3);
                        x.Insert(x1 - 1, calc);
                        z = x.Count;
                    }
                    else
                    {
                        int x1 = x.IndexOf("-");
                        calc = subtraction(x1, x);
                        x.RemoveRange(x1 - 1, 3);
                        x.Insert(x1 - 1, calc);
                        z = x.Count;
                    }
                }
                else
                {
                    if (question.Contains("+"))
                    {
                        int x1 = x.IndexOf("+");
                        calc = plus(x1, x);
                        x.RemoveRange(x1 - 1, 3);
                        x.Insert(x1 - 1, calc);
                        z = x.Count;
                    }
                    else
                    {
                        int x1 = x.IndexOf("-");
                        calc = subtraction(x1, x);
                        x.RemoveRange(x1 - 1, 3);
                        x.Insert(x1 - 1, calc);
                        z = x.Count;
                    }
                }
            }
            return x;
        }

        // calc multiply function
        public static string multiply(int index, List<string> mylist)
        {
            string x = mylist[index - 1].ToString();
            string y = mylist[index + 1].ToString();
            decimal c = decimal.Parse(x) * decimal.Parse(y);
            string d = c.ToString();
            return d;
        }

        //calc divide function
        public static string divide(int index, List<string> mylist)
        {
            string x = mylist[index - 1].ToString();
            string y = mylist[index + 1].ToString();
            decimal c = decimal.Parse(x) / decimal.Parse(y);
            string d = c.ToString();
            return d;
        }

        //calc plus functino
        public static string plus(int index, List<string> mylist)
        {
            string x = mylist[index - 1].ToString();
            string y = mylist[index + 1].ToString();
            decimal c = decimal.Parse(x) + decimal.Parse(y);
            string d = c.ToString();
            return d;
        }

        //calc subtraction function
        public static string subtraction(int index, List<string> mylist)
        {
            string x = mylist[index - 1].ToString();
            string y = mylist[index + 1].ToString();
            decimal c = decimal.Parse(x) - decimal.Parse(y);
            string d = c.ToString();
            return d;
        }

        // make string to List
        public static List<string> stringToList(string cal1)
        {
            var inputList = new List<string>();
            char[] characters = cal1.ToCharArray();
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i].ToString() == variable)
                {
                    characters[i] = 'x';
                }
            }

            string fistchar = "";
            for (int i = 0; i < characters.Length; i++)
            {
                if (char.IsLetterOrDigit(characters[i]) == true)
                {
                    fistchar += characters[i];
                }
                else if (IsOperator(characters[i].ToString()))
                {
                    if (fistchar != "")
                    {
                        inputList.Add(fistchar);
                    }
                    inputList.Add(characters[i].ToString());
                    fistchar = "";
                }
            }
            if (fistchar != "")
            {
                inputList.Add(fistchar);
            }

            while (IsOperator(inputList.Last()))
            {
                inputList.RemoveAt(inputList.Count - 1);
            }

            return inputList;
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("please enter an equation furmula:");
                string input;
                input = Console.ReadLine();

                try
                {
                    string removeSpaceInput = input.Replace(" ", "");
                    if (!removeSpaceInput.Contains("="))
                    {
                        Console.Write("please enter =");
                        Environment.Exit(-1);
                    }
                    char[] character = removeSpaceInput.ToCharArray();
                    Isvarible(character);
                    string[] cal = removeSpaceInput.Split('=');

                    Left = stringToList(cal[0]);
                    Right = stringToList(cal[1]);

                    if (Left[0] == "-")
                    {
                        Left.Insert(0, "0");
                    }
                    if (Right[0] == "-")
                    {
                        Right.Insert(0, "0");
                    }
                    Left = checkfrom(Left);
                    Right = checkfrom(Right);

                    //if has () reduce them use removeParenthesis function
                    if (Right.Contains("(") && Right.Contains(")"))
                    {
                        removeParenthesis(Right);
                    }
                    if ((!Right.Contains("(") && Right.Contains(")")) || (Right.Contains("(") && !Right.Contains(")")))
                    {
                        Console.WriteLine("has input in wrong");
                        Environment.Exit(-1);
                    }

                    if (Left.Contains("(") && Left.Contains(")"))
                    {
                        removeParenthesis(Left);
                    }

                    if ((!Left.Contains("(") && Left.Contains(")")) || (Left.Contains("(") && !Left.Contains(")")))
                    {
                        Console.WriteLine("hasr input in wrong");
                        Environment.Exit(-1);
                    }

                    //function is to exchange numbers and unknown variables to both sides of the equation
                    if (!cal[0].Contains("x") && cal[1].Contains("x"))
                    {
                        List<string> change = new List<string>();
                        change = Right;
                        Right = Left;
                        Left = change;
                        moveDToRight(Left, Right);
                    }
                    else
                    {
                        moveXToLeft(Left, Right);
                        moveDToRight(Left, Right);
                    }

                    string result = operationList(Right);
                    decimal Divisor = 1;
                    string Divisor33 = "";
                    List<string> Divisor34 = new List<string>();
                    List<string> LeftDivisor = new List<string>();

                    if (Left.Count >= 1)
                    {
                        for (int i = 0; i < Left.Count; i++)
                        {
                            if (Left[i] == "x")
                            {
                                Left.Remove(Left[i]);
                                Left.Insert(i, "1x");
                            }
                            Divisor33 += Left[i];
                        }
                        string[] xs = Divisor33.Split('x');
                        string u = "";
                        for (int j = 0; j < xs.Length; j++)
                        {
                            if (xs[j] != "")
                            {
                                u += xs[j];
                            }
                        }

                        Left = stringToList(u);
                        string xi = operationList(Left);
                        Divisor = decimal.Parse(xi);
                        decimal fianl = decimal.Parse(result) / Divisor;
                        Console.WriteLine("calc:{0}", fianl);
                    }
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Can not divide by zero");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Out of integer Range");
                }
            }
        }
    }
}





