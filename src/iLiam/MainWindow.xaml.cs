using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iLiam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _Score;
        private int _Attempts = 0;
        private const int MAX_ATTEMPTS = 5;

        public int Attempts
        {
            get { return _Attempts; }
            set
            {
                _Attempts = value;
                tbAttempts.Text = $"Tries Left: {MAX_ATTEMPTS - Attempts}";
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();
            this.NewQuestion();
            this.Score = 0;
            this.txtAnswer.KeyDown += new KeyEventHandler(this.TxtAnswer_KeyDown1);
            base.GotFocus += new RoutedEventHandler(this.MainWindow_GotFocus);
            //this.txtAnswer.Focus();
            Loaded += (sender, e) => this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void CheckAnswer()
        {
            if (this.Question.Validate(this.txtAnswer.Text))
            {
                int num = this.Score + 1;
                this.Score = num;
                if (num >= Required)
                {
                    base.Close();
                    EntryPoint.RunGame();
                }
                else
                {
                    this.NewQuestion();
                }
            }
            else
            {
                this.txtAnswer.Text = "";
                if(++this.Attempts >= MAX_ATTEMPTS)
                {
                    this.txtAnswer.IsEnabled = false;
                    //this.tbAttempts.Text = "Wait for new Question";
                    this.tbQuestion.Text = "Wait for Question";
                    using (BackgroundWorker worker = new BackgroundWorker())
                    {
                        worker.DoWork += (object sender, DoWorkEventArgs e) => {
                            System.Threading.Thread.Sleep(3000);
                        };
                        worker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
                        {
                            this.NewQuestion();
                            this.txtAnswer.IsEnabled = true;
                        };
                        worker.RunWorkerAsync();
                    }
                }
            }
        }

        private void MainWindow_GotFocus(object sender, RoutedEventArgs e)
        {
            this.txtAnswer.Focus();
        }

        private void NewQuestion()
        {
            this.Attempts = 0;
            this.Question = MathQuestion.GenerateQuestion();
            this.txtAnswer.Text = "";
            this.tbQuestion.Text = this.Question.ToString();
        }
        
        private void TxtAnswer_KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.CheckAnswer();
            }
        }

        private IQuestion Question { get; set; }

        public static int Required { get; set; }
        public int Score
        {
            get
            {
                return this._Score;
            }
            set
            {
                this._Score = value;
                this.tbScore.Text = $"Score {value} / {Required}";
            }
        }
    }
}
