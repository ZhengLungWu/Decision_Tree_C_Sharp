using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DecisionTree
{

    class DecisionTree
    {
        /*the thory of decision tree
         * https://ocw.mit.edu/courses/sloan-school-of-management/15-097-prediction-machine-learning-and-statistics-spring-2012/lecture-notes/MIT15_097S12_lec08.pdf
         *
         *
         * there are many types of decision tree algorithms, like ID3,C4.5,C5.0.... I initially choose c4.5 .
         * the references of various algorithms
         *  http://saiconference.com/Downloads/SpecialIssueNo10/Paper_3-A_comparative_study_of_decision_tree_ID3_and_C4.5.pdf
         *   
         *   pruning of C4.5:
         *   binominal distribution:
         *   https://en.wikipedia.org/wiki/Binomial_distribution
         *   
         */
        public DecisionTree(double[][]input_values,int[]labels)
        {
           
            
           
        }

        public DecisionTree(string[][] input_strings, int[] labels)
        {
            List<Strings_Label> input_items = new List<Strings_Label>();
            for (int i = 0; i < input_strings.Length; i++)
            {
                input_items.Add(new Strings_Label(input_strings[i], labels[i]));
            }
        }



    }

    class Node_ID3
    {
       
     
        public List<Values_Label>[] OUTPUT_GROUPS;        
        private List<Values_Label> ITEM_LIST;
        public double GAIN;
        public Node_ID3 (double[][]inputvalues,int[]labels,int Feature_index,double[]Seperate_values)
        {                      
            ITEM_LIST = new List<Values_Label>();
            for (int i = 0; i < inputvalues.Length; i++)
            {
                ITEM_LIST.Add(new Values_Label(inputvalues[i], labels[i]));
            }
           OUTPUT_GROUPS=Spliting(ITEM_LIST, Feature_index, Seperate_values);
           GAIN = ID3_Gain(ITEM_LIST, OUTPUT_GROUPS);
        }
        public Node_ID3(List<Values_Label>input_values_label, int Feature_index, double[] Seperate_values)
        {            
            ITEM_LIST =input_values_label;            
            OUTPUT_GROUPS = Spliting(ITEM_LIST, Feature_index, Seperate_values);
            GAIN = ID3_Gain(ITEM_LIST, OUTPUT_GROUPS);
        }
        private static List<Values_Label>[] Spliting(List<Values_Label>item_list,int Feature_index,double[] Seperate_values)
        {
            List<Values_Label>[] temps = new List<Values_Label>[Seperate_values.Length + 1];
            for (int k = 0; k < temps.Length; k++)
            {
                temps[k] = new List<Values_Label>();
            }

            for (int i = 0; i < item_list.Count; i++)
            {
                if (item_list[i].VALUES[Feature_index] <= Seperate_values[0])
                {
                    temps[0].Add(item_list[i]);
                }
                else if (item_list[i].VALUES[Feature_index] >= Seperate_values[Seperate_values.Length - 1])
                {
                    temps[temps.Length-1].Add(item_list[i]);
                }
                else
                {
                    for (int j = 1; j < Seperate_values.Length; j++)
                    {
                        if (item_list[i].VALUES[Feature_index] >= Seperate_values[j - 1] && item_list[i].VALUES[Feature_index] < Seperate_values[j])
                        {
                            temps[j].Add(item_list[i]);
                        }
                    }
                }
            }
            return temps;
        }
        private static double[] Probability_Evaluation(int[]Labels)
        {
            double[] probs = new double[2];
            for (int i = 0; i < Labels.Length; i++)
            {
                if (Labels[i] == 0)
                {
                    probs[0] += 1;
                }
                else
                {
                    probs[1] += 1;
                }
            }
            probs[0] /= Labels.Length;
            probs[1] /= Labels.Length;
            return probs;
        }
        private static double[] Probability_Evaluation(List<Values_Label> objs)
        {
            double[] probs = new double[2];
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i].LABEL == 0)
                {
                    probs[0] += 1;
                }
                else
                {
                    probs[1] += 1;
                }
            }
            probs[0] /= objs.Count;
            probs[1] /= objs.Count;
            return probs;
        }
        private static double ID3_Gain(List<Values_Label>pre_split_data,List<Values_Label>[]split_result)
        {
            double[] probs_pre = Probability_Evaluation(pre_split_data);
            double entropy_pre = Entropy_Evaluation(probs_pre);           
            double[] entropy_reses = new double[split_result.Length];
            double Gain = entropy_pre;
            for (int i = 0; i < split_result.Length; i++)
            {
                entropy_reses[i] = Entropy_Evaluation(Probability_Evaluation(split_result[i]));
                Gain += (-probs_pre[i] * entropy_reses[i]);
            }
            return Gain;
        }
        private static double Entropy_Evaluation(double[]probalities)
        {
            double res = 0.0;
            for (int i = 0; i < probalities.Length; i++)
            {
                res += probalities[i] * Math.Log(probalities[i], 2);
            }
            return -res;
        }
        private static double[] GenerateSeperator(double[][] input_values, int Feature_index, int SeperatorAmount)
        {
            List<double> Feature_ = new List<double>();
            for (int i = 0; i < input_values.Length; i++)
            {
                Feature_.Add(input_values[i][Feature_index]);
            }
            Feature_.Sort();
            double[] seperator = new double[SeperatorAmount];
            for (int i = 0; i < seperator.Length; i++)
            {
                seperator[i] = Feature_[(i + 1) * Feature_.Count / (SeperatorAmount + 1)];
            }
            return seperator;
        }
    }
    enum AlgorithmType
    {
        ID3,
        C45
    }

    class Node_C45
    {
        
        private List<Strings_Label> ITEM_LIST;

        public List<List<Strings_Label>> OUTPUT_GROUPPED_ITEMS;

        public Node_C45(List<Strings_Label> input_strings_label)
        {
            this.ITEM_LIST = input_strings_label;
           int FeatureAmount=input_strings_label[0].STRINGS.Length;
            double[] GainRatios = new double[FeatureAmount];
            List<List<Strings_Label>>[] groupped_items_byFI = new List<List<Strings_Label>>[FeatureAmount];
            double gainr = 0.0;
            int indexofG = 0;
            for (int i = 0; i < FeatureAmount; i++)
            {
                
              GainRatios[i]= GainRatio(input_strings_label, i,out groupped_items_byFI[i]);
                if (i == 0) gainr = GainRatios[i];
                else
                {
                    if (gainr < GainRatios[i])
                    {
                        gainr = GainRatios[i];
                        indexofG = i;
                    }
                }
            }

            if (Pruning(input_strings_label, groupped_items_byFI[indexofG]))
            {
                OUTPUT_GROUPPED_ITEMS = null;
            }
            else
            {
                OUTPUT_GROUPPED_ITEMS = groupped_items_byFI[indexofG];
            }

             


        }
        private static double[] Probabilities(List<Strings_Label> SUB, int FeatureIndex,out List<List<Strings_Label>>sub_items_list)
        {
            string temp = "";
            sub_items_list = new List<List<Strings_Label>>();

            List<string> items = new List<string>();
            List<int> Amounts = new List<int>();
            for (int i = 0; i < SUB.Count; i++)
            {
                temp = SUB[i].STRINGS[FeatureIndex];
                if (!items.Contains(temp))
                {
                    items.Add(temp);
                    Amounts.Add(1);
                    sub_items_list.Add(new List<Strings_Label>());
                    sub_items_list[sub_items_list.Count - 1].Add(SUB[i]);
                }
                else
                {
                    for (int j = 0; j < items.Count; j++)
                    {
                        if (temp == items[j])
                        {
                            Amounts[j] += 1;
                            sub_items_list[j].Add(SUB[i]);
                        }
                    }
                }
            }
            double[] probs = new double[Amounts.Count];
           
            for (int i = 0; i < probs.Length; i++)
            {
                probs[i] = Amounts[i] / SUB.Count;
               
            }
            return probs;

        }
        private static double Entropy(List<Strings_Label> SUB, int FeatureIndex)
        {
            string temp = "";
            List<string> items = new List<string>();
            List<int> Amounts = new List<int>();
            for (int i = 0; i < SUB.Count; i++)
            {
                temp=SUB[i].STRINGS[FeatureIndex];
                if (!items.Contains(temp))
                {
                    items.Add(temp);
                    Amounts.Add(1);
                }
                else {
                    for (int j = 0; j < items.Count; j++)
                    {
                        if (temp == items[j]) Amounts[j] += 1;
                    }
                }
            }
            double[] probs = new double[Amounts.Count];
            double ent = 0;
            for (int i = 0; i < probs.Length; i++)
            {
                probs[i]=Amounts[i] / SUB.Count;
                ent += -probs[i] * Math.Log(probs[i], 2);
            }
            return ent;
        }

        private static double H_function(double A)
        {
            return -A * Math.Log(A, 2) - (1 - A) * Math.Log(1 - A, 2);
        }

        private static double H_function(List<Strings_Label>S)
        {
            double counter = 0;
            for (int i = 0; i < S.Count; i++)
            {
                if (S[i].LABEL == 1)
                {
                    counter++;
                }
            }
            counter /= S.Count;
            return H_function(counter);
        }

        private static double Gain(List<Strings_Label> S, int FeatureIndex,out List<List<Strings_Label>> groupped_items)
        {
            double g = H_function(S);
            
            double[] probs = Probabilities(S, FeatureIndex, out groupped_items);
            for (int i = 0; i < groupped_items.Count; i++)
            {
                double positve = 0.0;
                for (int j = 0; j < groupped_items[i].Count; j++)
                {
                    if (groupped_items[i][j].LABEL == 1)
                    {
                        positve++;
                    }
                }
                positve /= groupped_items[i].Count;
                g += -probs[i] * H_function(positve);
            }
            return g;
        }

        private static double SplitInfo(List<Strings_Label>S,List<List<Strings_Label>>groupped_items)
        {
            double res = 0.0;
            for (int i = 0; i < groupped_items.Count; i++)
            {
                res += -(groupped_items[i].Count / S.Count) * Math.Log((groupped_items[i].Count / S.Count), 2);
            }         
            return res;
        }
        private static double GainRatio(List<Strings_Label> S, int FeatureIndex, out List<List<Strings_Label>> groupped_items)
        {
            return Gain(S, FeatureIndex, out groupped_items) / SplitInfo(S, groupped_items);
        }

        private static double Binominal_distribution(int mis_class,int total,double prob)
        {
            if (total < mis_class) { int temp = mis_class; mis_class = total; total = temp; }
            return Math.Pow(prob, mis_class) * Math.Pow(1 - prob, total - mis_class) * Factorial(total) / (Factorial(mis_class) * Factorial(total - mis_class));
        }

       private static double CDF(int N_total,int M_sub,double prob)
        {
            double temp = 0;
            for (int i = 0; i <= M_sub; i++)
            {
                temp += Binominal_distribution(i, N_total, prob);
            }
            return temp;
        }
         private static double Prob_evaluation(int N_total, int M_sub, double Alpha)
        {
            double tol = 1e-5;
            double p = 0.0;
            
            double res=CDF(N_total, M_sub, p);
            double temp = Alpha - res;
            while (Math.Abs(Alpha - res) > tol)
            {
                if ((temp > 0 && Alpha - res < 0) || (temp < 0 && Alpha - res > 0))
                {
                    p -= tol/10;
                    Console.WriteLine(p);
                }
               
                else
                {
                    p += tol;
                    temp = Alpha - res;
                }
                
                res = CDF(N_total, M_sub, p);
               //
            }
            return p;
        }

       private static double UpperBound(List<List<Strings_Label>> groupped_items)
        {
            int total = 0;
            int[] group_ele_amount = new int[groupped_items.Count];
            int[] positve_amount = new int[groupped_items.Count];
            double[] probs = new double[groupped_items.Count];
            double result = 0;
            for (int i = 0; i < groupped_items.Count; i++)
            {
                for (int j = 0; j < groupped_items[i].Count; j++)
                {
                    total++;
                    group_ele_amount[i]++;
                    if (groupped_items[i][j].LABEL == 1)
                    {
                        positve_amount[i]++;
                    }
                }
                probs[i] = Prob_evaluation(groupped_items[i].Count, positve_amount[i],0.25);
                result += groupped_items[i].Count * probs[i];
            }
            return result / total;
        }
       private static double UpperBound(List<Strings_Label> S)
        {

            int positive_amount = 0;
            for (int i = 0; i < S.Count; i++)
            {
                if (S[i].LABEL == 1) positive_amount++;
            }


          return  Prob_evaluation(S.Count, positive_amount, 0.25)/S.Count;
        }
      private static Int64 Factorial(int a)
      {
            Int64 temp = 1;
            for (; a > 0; a--)
            {
                temp *= a;
            }
            return temp;
      }
        private static bool Pruning(List<Strings_Label>S,List<List<Strings_Label>>groupped_items)
        {
            return UpperBound(S) > UpperBound(groupped_items) ? false : true;
        }
    }


    class Values_Label
    {
        public double[] VALUES;
        public int LABEL;
        public Values_Label(double[] values, int label)
        {
            this.VALUES = values;
            this.LABEL = label;
        }
    }

    class Strings_Label
    {
        public string[] STRINGS;
        public int LABEL;
        public Strings_Label(string[] strings, int label)
        {
            this.STRINGS = strings;
            this.LABEL = label;
        }

    }


}
