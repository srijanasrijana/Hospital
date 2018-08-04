using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
   public static class AmountToWords
    {
        //  public  static class Extension
         //   {
                public static int ToInt(this string str)
                {
                    int output;
                    int.TryParse(str, out output);
                    return output;
                }
           // }

        static List<string> oneTo19Text = new List<string> {
                                            "Zero", "One" , "Two", "Three", "Four", 
                                              "Five", "Six", "Seven", "Eight", 
                                              "Nine", "Ten" , "Eleven" , "Twelve",
                                              "Thirteen", "Fourteen", "Fifteen" , 
                                              "Sixteen" , "Seventeen", "Eighteen" , "Nineteen"
                                            };

        static Dictionary<int, string> tensDigit =
               new Dictionary<int, string>() 
                                                    {  
                                                        { 2,"Twenty"},
                                                        { 3,"Thirty"},
                                                        { 4,"Fourty"},
                                                        { 5,"Fifty"},
                                                        { 6,"Sixty"},
                                                        { 7,"Seventy"},
                                                        { 8,"Eighty"},
                                                        { 9,"Ninety"}
                                                    };

        static Dictionary<int, string> HundredDigit =
               new Dictionary<int, string>() 
                                                {  
                                                    { 3,"Hundred"},
                                                    { 4,"Thousand"},
                                                    { 6,"Lakhs"},
                                                    { 8,"Crore"},
                                                    { 10,"Arb"},
                                                    { 12,"Kharb"}
                                                };
        public static string ConvertNumberAsText(string no)
        {
            string num = no;
            string paisa="";
            int present=0;
            int rupees = 1;

            string result = string.Empty;
            for (int i = 1; i == 1; i = present)
            {
                #region to run loop twice
                if (num.Contains("."))
                {
                    string[] splitmoney = num.Split('.');
                    num = splitmoney[0];
                    num = Convert.ToInt64(num) == 0 ? num = "0" : num;
                    paisa = splitmoney[1];
                    //////////////////
                   
                    if (paisa.Length > 1)
                    {
                        paisa = paisa.Substring(0, 2);
                    }
                    else if (paisa.Length == 1)
                    {
                        paisa = paisa + "0";
                    }
                    ////////////////////
                    present = 1;
                }
                else
                {
                    if (present == 1)
                    {
                        if (Convert.ToInt32(paisa) == 0)
                        {
                            paisa = "";
                        }
                        num = paisa;
                    }
                    else
                       num = Convert.ToInt64(num)==0?num = "0":num = no;
                       // num = no;
                    present = 5;
                }
                #endregion

                int numberLength = num.ToString().Length;
                string numberString = num.ToString();
                int position = numberLength;
                if (numberLength == 1)
                {
                    if (present == 1)
                    {
                        result = oneTo19Text[Convert.ToInt32(num)];
                        if (Convert.ToInt32(num) != 0 && Convert.ToInt32(paisa)!=0 )
                        {
                            result = result + " Rupees and ";
                        }
                        else
                        {
                            result = result + " Rupees.";
                        }
                        rupees = 0;
                    }
                    else
                    {

                        result = oneTo19Text[Convert.ToInt32(num)];
                        result = result + " Rupees.";
                    }
                    //return oneTo19Text[Convert.ToInt32(num)];
                }
                else
                {
                    // string result = string.Empty;
                    int number = 0;
                    // loop the position in number string
                    #region start for loop
                    for (int startPosition = 0; startPosition < numberLength; startPosition++)
                    {
                        // check the position is equalent to hundred,
                        // thousand and lakks or its hundred ,tenthousand .....
                        Dictionary<int, string> getHundtedWord = HundredDigit.Where(
                           p => p.Key == position).ToDictionary(p => p.Key, p => p.Value);
                        int chkzero = 0;
                        if (getHundtedWord.Count == 0)
                        {                           
                            number = numberString.Substring(startPosition, 1).ToInt();
                            if (number < 2)
                            {
                                number = numberString.Substring(startPosition, 2).ToInt();
                                startPosition++;
                                position--;
                            }
                            else
                            {
                                result += " " + tensDigit[number];
                                startPosition++;
                                position--;
                                chkzero = 1;
                                number = numberString.Substring(startPosition, 1).ToInt();                               
                            }
                            if (number == 0)
                            {
                                result += " ";
                               // number = 1;
                            }
                            else
                            {
                                result += " " + oneTo19Text[number];
                                //if (number == 0)
                                    
                            }
                            if ( position > 2)
                            {
                                if (chkzero == 1)
                                    number = 1;
                                 if (number > 0 )                                
                                    result += " " + HundredDigit[position];
                            }
                        }
                        else
                        {
                            number = numberString.Substring(startPosition, 1).ToInt();
                            // result += " " + oneTo19Text[number];

                            if (number == 0)
                            {
                                result += " ";
                                //number = 1;
                            }
                            else
                            {
                                result += " " + oneTo19Text[number];                               
                            }

                            if (position > 2)
                            {
                                if (chkzero == 1)
                                    number = 1;
                                if (number > 0)
                                result += " " + HundredDigit[position];
                            }
                        }
                    #endregion
                        position--;
                    }
                    if (no.Contains('.'))
                    {
                        //string Rupees = string.Empty;
                        if (rupees == 1)
                        {
                            if ( Convert.ToInt32(paisa) == 0)
                            {
                                result = result + " Rupees.";
                            }
                            else
                            {
                                result = result + " Rupees and ";
                            }
                            rupees = 0;
                        }
                        else if (rupees == 0)
                        {
                            if (paisa == "")
                                paisa = "0";
                            if ( Convert.ToInt32(paisa) != 0)
                            {
                                result = result + " Paisa.";
                            }                           
                        }

                    }
                    else
                    {
                        result = result + " Rupees.";
                    }
                }
            }
            return result;         
            
        }
    }

     
}
