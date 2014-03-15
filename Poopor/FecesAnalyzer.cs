using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    class FecesAnalyzer
    {
        //Data field
        SQLiteFunctions db_function = new SQLiteFunctions();

        //Constructor
        public FecesAnalyzer()
        {
        }

        //Get 15 input data: color(string), shape(string), painLv(string), 
        //bloodAmt(string), weight(double), height(double), gender(string), age(int),
        //healthInfo1(boolean) >> FDR colon/ovarian/endo/breast, healthInfo2(boolean) >> USER inflemm/polyps, healthInfo3(boolean) >> smoking/drinking, 
        //healthInfo4(boolean) >> USR/FDR FAP HNPCC, healthInfo5(boolean) >> female, isMelena >> boolean, isHavingMedicine >> boolean
        public async Task<Dictionary<string, List<string>>> analyzeData(string color, string shape, string painLv, string bloodAmt,
            double weight, double height, string gender, int age, bool healthInfo1,
            bool healthInfo2, bool healthInfo3, bool healthInfo4, bool healthInfo5, bool isMelena, bool isHavingMedicine)
        {
            int general_sign_counter = 0;
            int anxious_sign_counter = 0;
            List<string> userCancerSignMsg = new List<string>();
            double bmi_val = 0;

            //-------- General sign checklist here
            if (age >= 50)
            {
                general_sign_counter++;
                userCancerSignMsg.Add("Age is equal or higher than 50.");
            }

            if (healthInfo3) //smoking/drinking
            {
                general_sign_counter++;
                userCancerSignMsg.Add("You are addicted to drinking alcohol or smoking.");
            }

            //calculate BMI according to WHO
            bmi_val = weight / ((height / 100.00) * (height / 100.00));
            if (bmi_val > 25.00)
            {
                general_sign_counter++;
                userCancerSignMsg.Add("You are obesity.");
            }

            //find total bowel movement, melena, and pain level
            List<int> countMelenaAndPainLevel = db_function.CountMelenaAndPainLevel(SessionManagement.GetEmail());
            int totalBowelMovementTimesIn_7_Days = countMelenaAndPainLevel[0];
            int melenaCount = countMelenaAndPainLevel[1];
            int severePainCount = countMelenaAndPainLevel[2];
            int worstPainCount = countMelenaAndPainLevel[3];

            //Check melena feces records
            if (melenaCount > 1)
            {
                general_sign_counter++;
                //- Retrieve feces records back from now 
                userCancerSignMsg.Add("Seven days ago, your feces were melena for " + melenaCount + " bowel movement times(out of " + totalBowelMovementTimesIn_7_Days + ")");
            }

            //Check pain level of feces records
            if (severePainCount >= 3 || worstPainCount >= 1)
            {
                general_sign_counter++;
                //- Retrieve feces records back from now 
                if (severePainCount == 0)
                    userCancerSignMsg.Add("Seven days ago, you feel pain in bowel movement with Severe level " + severePainCount + " times(out of " + totalBowelMovementTimesIn_7_Days + ")");
                else if (worstPainCount == 0)
                    userCancerSignMsg.Add("Seven days ago, you feel pain in bowel movement with Worst level " + worstPainCount + " times(out of " + totalBowelMovementTimesIn_7_Days + ")");
                else
                    userCancerSignMsg.Add("Seven days ago, you feel pain in bowel movement with Worst level " + worstPainCount + " times, Severe level " + severePainCount +
" times(out of " + totalBowelMovementTimesIn_7_Days + ")");
            }

            //Check Constipation and Diarrhea
            if (db_function.IsConstipationSwapDiarrheaPattern(SessionManagement.GetEmail()))
            {
                general_sign_counter++;
                //- Retrieve feces records back from now 
                userCancerSignMsg.Add("Your feces records have shown that you are constipation swap with diarrhea.");
            }

            //-------- Anxious sign checklist here
            if (healthInfo1) //FDR colon/ovarian/endo/breast
            {
                anxious_sign_counter++;
                userCancerSignMsg.Add("Your family members(first-degree relative) have been diagnosed with colon, rectum, ovarian, endometrium, or breast cancer.");
            }

            if (healthInfo2) //USER inflemm/polyps
            {
                anxious_sign_counter++;
                userCancerSignMsg.Add("You have been diagnosed with inflammatory bowel disease or colorectal polyps.");
            }

            if (healthInfo4) //USER/FDR FAP HNPCC
            {
                anxious_sign_counter++;
                userCancerSignMsg.Add("You or your family members(first-degree relative) have been diagnosed with FAP(Familial adenometous polyposis) or HNPCC(Hereditary nonpolyposis colon cancer).");
            }

            if (healthInfo5) //Female User: ovarian/endometrium/breast cancer
            {
                anxious_sign_counter++;
                userCancerSignMsg.Add("You have been diagnosed with ovarian, endometrium or breast cancer.");
            }

            //Return these value to be shown in Result Page
            return generateResultDictionary((anxious_sign_counter > 1) ? true : false, generateUserCancerSign(general_sign_counter, anxious_sign_counter), userCancerSignMsg,
                db_function.GetColorMeaning(color), db_function.GetShapeMeaning(shape), isRecommend(color, shape), db_function.GetShortRec(color, shape),
                db_function.GetLongRec(color, shape), isConstipation(shape), isDiarrhea(shape));

        }

        //Check is recomm or not method
        private bool isRecommend(string color, string shape)
        {
            bool isPerfectColor = color.Equals("Medium brown", StringComparison.Ordinal);
            bool isPerfectShape = shape.Equals("Smooth soft snake", StringComparison.Ordinal);
            return (isPerfectColor == true && isPerfectShape == true) ? true : false;
        }


        //Check is constipation 
        private bool isConstipation(string shape)
        {
            bool isShape1 = shape.Equals("Separated hard lumps", StringComparison.Ordinal);
            bool isShape2 = shape.Equals("Lumpy sausage", StringComparison.Ordinal);
            return (isShape1 == true || isShape2 == true) ? true : false;
        }


        //Check is diarrhea
        private bool isDiarrhea(string shape)
        {
            bool isShape6 = shape.Equals("Mushy and fluffy pieces", StringComparison.Ordinal);
            bool isShape7 = shape.Equals("Entirely liquid", StringComparison.Ordinal);
            return (isShape6 == true || isShape7 == true) ? true : false;
        }


        //User cancer sign analyzer
        private string generateUserCancerSign(int general_sign_counter, int anxious_sign_counter)
        {
            string userCancerSignStr = "";
            if (general_sign_counter == 0 && anxious_sign_counter == 0) userCancerSignStr = "none";
            else if (anxious_sign_counter > 0) userCancerSignStr = "anxious";
            else if (general_sign_counter > 0) userCancerSignStr = "general";
            return userCancerSignStr;
        }

        //Ask more info in case of Anxious sign occur
        //...Boss help to do this pattern


        //Prepare dictionary as a result to send to show the result of calculation
        private Dictionary<string, List<string>> generateResultDictionary(bool isGoAsk, string userCancerSign, List<string> userCancerSignMsg, List<string> userPoopColorMeaning,
            List<string> userPoopShapeMeaning, bool isRecommend, List<string> userShortRecommendation, List<string> userLongRecommendation, bool isConstipation,
            bool isDiarrhea)
        {
            try
            {
                Dictionary<string, List<string>> resultDictionary = new Dictionary<string, List<string>>();
                List<string> necessaryInfo = new List<string>();
                necessaryInfo.Add(isGoAsk.ToString());
                necessaryInfo.Add(userCancerSign);
                necessaryInfo.Add(isRecommend.ToString());
                necessaryInfo.Add(isConstipation.ToString());
                necessaryInfo.Add(isDiarrhea.ToString());
                resultDictionary.Add("NecessaryInfo", necessaryInfo);
                resultDictionary.Add("UserCancerSignMsg", userCancerSignMsg);
                resultDictionary.Add("UserPoopColorMeaning", userPoopColorMeaning);
                resultDictionary.Add("UserPoopShapeMeaning", userPoopShapeMeaning);
                resultDictionary.Add("UserShortRecommendation", userShortRecommendation);
                resultDictionary.Add("UserLongRecommendation", userLongRecommendation);
                return resultDictionary;
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (System.Reflection.TargetInvocationException c)
            {
                Debug.WriteLine(c.Message);
            }
            return null;
        }
    }
}
