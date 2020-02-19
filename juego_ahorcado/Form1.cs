using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace juego_ahorcado
{

    

    public partial class Form1 : Form
    {

        int wordsListLenght;
        int timerCounter;
        int timerInterval;
        int levelNumber;
        int intervalDiffBetweenLvl;
        
        List<int> selectedIndexes;
        
        string lowBarWord;
        string selectedWord;

        Random rand;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initializing variables
            rand = new Random();
            selectedIndexes = new List<int>();
            lowBarWord = "";

            // Asigning values
            wordsListLenght = wordsLb.Items.Count;
            timerCounter = 60;
            timerInterval = 1000;
            levelNumber = 1;
            intervalDiffBetweenLvl = 100;

            // Setup the game
            disableTimer();

            //Give styles to controls
            Button initResetButton = this.Controls.Find("startResetBtn", true)[0] as Button;
            initResetButton.BackColor = System.Drawing.Color.Transparent;
            initResetButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            initResetButton.FlatAppearance.BorderSize = 2;
        }

        private void onMouseEnter_startResetBtn(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            btn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btn.ForeColor = System.Drawing.Color.Black;
        }

        private void onMouseLeave_startResetBtn(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            btn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            btn.ForeColor = System.Drawing.Color.White;

        }

        private void wordsLb_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Every time an item is selected, change the label to lowBarWord
            selectedWord = wordsLb.Items[wordsLb.SelectedIndex].ToString();

            lowBarWord = getLowBarWord(selectedWord);
            currentWordLbl.Text = lowBarWord;
        }

        private void startResetBtn_Click(object sender, EventArgs e)
        {
            string initGameLbl = "Iniciar Juego";
            string resetGameLbl = "Reiniciar Juego";

            if(startResetBtn.Text == initGameLbl)
            {
                startResetBtn.Text = resetGameLbl;
                selectRandomWord();
                enableTimer();
            }
            else
            {
                disableTimer();
                if(MessageBox.Show("¿Estás seguro de reiniciar?",
                                "Reiniciar Juego",
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    // If the answer is YES
                    resetGameValues();
                    timerLbl.Text = "Tiempo: " + timerCounter.ToString();
                    startResetBtn.Text = resetGameLbl;
                    selectRandomWord();
                }

                enableTimer();
            }
        }

        void resetGameValues()
        {
            // Initializing variables
            rand = new Random();
            selectedIndexes = new List<int>();
            lowBarWord = "";

            // Asigning values
            wordsListLenght = wordsLb.Items.Count;
            timerCounter = 60;
            timerInterval = 1000;
            levelNumber = 1;
            intervalDiffBetweenLvl = 100;

            timer1.Interval = timerInterval;    
        }

        void showFinishGame()
        {
            string resetGameLbl = "Reiniciar Juego";

            if (MessageBox.Show("Ganaste! Quieres volver a jugar?",
                                "FELICIDADES!!!!!!",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information) == DialogResult.Yes)
            {
                // Do if answer is YES
                resetGameValues();
                timerLbl.Text = "Tiempo: " + timerCounter.ToString();
                startResetBtn.Text = resetGameLbl;
                selectRandomWord();
                enableTimer();
            }
            else
            {
                // Do if answer is NO
                Close();
            }
        }

        void showGameOver()
        {
            string resetGameLbl = "Reiniciar Juego";

            if (MessageBox.Show("¿Quieres volver a intentarlo?",
                                "Perdiste! :(",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Do if answer is YES
                resetGameValues();
                timerLbl.Text = "Tiempo: " + timerCounter.ToString();
                startResetBtn.Text = resetGameLbl;
                selectRandomWord();
                enableTimer();
            } 
            else
            {
                // Do if answer is NO
                Close();
            }
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            Button buttonPressed = (sender as Button);
            char letterPressed = buttonPressed.Name[0];

            checkLetterInWord(letterPressed);
        }

        void checkLetterInWord(char letter)
        {
            for(int i = 0; i < selectedWord.Length; i++)
            {
                if(selectedWord[i] == letter)
                {
                    lowBarWord = lowBarWord.Remove((i * 2), 1).Insert(i * 2, letter.ToString());
                    currentWordLbl.Text = lowBarWord;
                }
            }

            if (checkIfWordIsGuessed(lowBarWord))
            {
                disableTimer();

                if (MessageBox.Show("Presiona aceptar para ir al siguiente nivel", "Felicidades!",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Information
                 ) == DialogResult.OK)
                {
                    // Make this if user pressed "Aceptar"
                    selectRandomWord();
                    addLvlDifficulty();
                    enableTimer();
                }
            }
            
        }

        bool checkIfWordIsGuessed(string wordToCheck)
        {
            return !wordToCheck.Contains("_");
        }

        void selectRandomWord()
        {
            bool isRepeatedWord;
            int randomIndex = rand.Next(wordsListLenght - 1);
            //Generates a random index between 0 and the words quantity minus one
            if(selectedIndexes.Count > 0)
            {
                isRepeatedWord = selectedIndexes.Contains(randomIndex);
            }
            else
            {
                isRepeatedWord = false;
            }

            if (isRepeatedWord)
            {
                selectRandomWord();
            }
            else
            {
                //Save the value so that it does not happen again
                wordsLb.SelectedIndex = randomIndex;
                selectedIndexes.Add(randomIndex);
            }
        }

        string getLowBarWord(string wordToConvert)
        {
            // We iterate every character and change to lowBarLbl
            string lowBar = "";
            string lowBarLbl = "_ ";
            int wordLenght = wordToConvert.Length;
            
            for(int i = 0; i < wordLenght; i++)
            {
                lowBar += lowBarLbl;
            }
            
            return lowBar;
        }

        void addLvlDifficulty()
        {
            if(levelNumber != 10)
            {
                levelNumber++;
                timerCounter = 60;
                timerInterval = 1000 - (intervalDiffBetweenLvl * levelNumber) - 100;

                if (timerInterval != 0)
                {
                    timer1.Interval = timerInterval;
                }
                else
                {
                    timer1.Interval = 100;
                }

                levelLbl.Text = "Nivel: " + levelNumber.ToString();
            } else
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerCounter--;
            timerLbl.Text = "Tiempo: " + timerCounter.ToString();
        
            if(timerCounter == 0)
            {
                disableTimer();
                showGameOver();
            }
        }

        void enableTimer()
        {
            timer1.Enabled = true;
        }

        void disableTimer()
        {
            timer1.Enabled = false;
        }

    }
}
