namespace iLiam
{

    using System;
    using System.Runtime.CompilerServices;

    internal class MathQuestion : IQuestion
    {
        private static Random random = new Random(DateTime.UtcNow.Millisecond);

        public static MathQuestion GenerateQuestion()
        {
            MathQuestion question = new MathQuestion();
            double num = random.NextDouble();
            if (num >= 0.9)
            {
                question.Number1 = random.Next(70, 100);
                question.Number2 = random.Next(11, 30);
            }
            else if (num > 0.75)
            {
                question.Number1 = random.Next(2, 8);
                question.Number2 = random.Next(2, 9);
                question.Number3 = 10 - question.Number1;
            }
            else if (num > 0.55)
            {
                //do
                //{
                    question.Number1 = random.Next(10, 100);
                    question.Number3 = question.Number1 + random.Next(7, 20);
                //}
                //while (Math.Abs(question.Number1 - question.Number3.Value) < 6);
                //question.Subtract = question.Number1 > question.Number3;
                return question;
            }
            else if (num >= 0.35)
            {
                question.Number1 = random.Next(1, 20) * 10;
                question.Number2 = random.Next(10, 50);
            }
            else
            {
                do
                {
                    question.Number1 = random.Next(10, 100);
                    question.Number2 = random.Next(3, 20);
                }
                while (Math.Abs(question.Number1 - question.Number2.Value) < 6);
            }

            question.Subtract = random.NextDouble() > 0.7;
            if (question.Subtract && question.Number2 > question.Number1)
            {
                int num1 = question.Number1;
                int num2 = question.Number2.Value;
                question.Number1 = num2;
                question.Number2 = num1;
            }
            return question;
        }

        public override string ToString()
        {
            if (this.Number2 == null)
            {
                if (this.Subtract)
                    return string.Format("{0} - ??? = {1}", this.Number1, this.Number3);
                return string.Format("{0} + ??? = {1}", this.Number1, this.Number3);
            }
            if (this.Number3 != null)
                return string.Format("{0} + {1} + {2} = ", this.Number1, this.Number2, this.Number3);
            if (this.Subtract)
            {
                return string.Format("{0} - {1} = ", this.Number1, this.Number2);
            }
            return string.Format("{0} + {1} = ", this.Number1, this.Number2);
        }

        public bool Validate(string Answer)
        {
            int result = 0;
            if (!int.TryParse(Answer, out result))
            {
                return false;
            }

            if(this.Number2 == null)
            {
                if(this.Subtract)
                    return this.Number1 - result == this.Number3;
                return this.Number1 + result == this.Number3;
            }

            if (this.Number3 != null)
            {
                return this.Number1 + this.Number2 + this.Number3 == result;
            }
            if (this.Subtract)
            {
                return ((this.Number1 - this.Number2) == result);
            }
            return ((this.Number1 + this.Number2) == result);
        }

        public int Number1 { get; set; }

        public int? Number2 { get; set; }

        public int? Number3 { get; set; }

        public bool Subtract { get; set; }
    }
}
