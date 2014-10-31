//Routine to process updating score, announcing scores

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class Scoring : MonoBehaviour
{
    public TextMesh[] Red = new TextMesh[3];     //The 4 Score HUD objects, in ascending order of Toolbox.Colors enum
    public TextMesh[] Blue = new TextMesh[3];
    public TextMesh[] Green = new TextMesh[3];
    public TextMesh[] Yellow = new TextMesh[3];
    public static GameObject Helper;        //Invisible scoring helper thinker gameobject    
    public static byte[] Score=new byte[5]; //Array of scores (0th is a dummy, in order to align with Color enum)
    
    void Start()
    {
        byte i;                         //Generic counter variable
        byte j;

        //Get the helper gameobject
        Helper = this.gameObject;
        Helper.light.enabled = false;   //Disable the score box light

        //Initz all scores to 0
        for (i = 0; i <= 4; i++)
        {
            Score[i] = 0;
        }
    }

    void Update()
    {

        if (Toolbox.Options.DBug==true)
        {
            string[] scores = new string[5];    //Array to hold string versions of scores

            //Convert all scores to string inside the array
            scores[0] = string.Empty;
            scores[1] = Score[1].ToString();
            scores[2] = Score[2].ToString();
            scores[3] = Score[3].ToString();
            scores[4] = Score[4].ToString();

            //Log the current scores
            Debug.Log("Score = " + scores[1] + " " + scores[2] + " " + scores[3] + " " + scores[4]);
        }
        UpdateHUD();

    }

    void UpdateHUD()
    {
        byte i;
        byte j;
        byte Hi;
        string Num;

        bool[] HiBitField =  //Bitfield determining who is tied. 0th field is True if no ties, otherwize false
        {
            false,false,false,false,false
        };

        string[] Words= new string[7];
        string[] Message=
        {
            "    ","GAME", "OVER", "TEAM", "TIES", "WINS"
        };

        string[] Teams=
        {
            "NULL", "RED ", "GRN", "BLUE", "YLOW"
        };

        //@To fix: Make messages appear with sometype of timer delay
        Hi=FindHiScore(true);
        if (Hi>=10)
        {
            Words[0] = Message[1];  //"Game"
            Words[1] = Message[2];  //"Over"
            Words[2] = Message[0];  //Pause

            FindTies(ref Score, ref HiBitField, Hi);    //Using that #, find the tie(s)

            if (HiBitField[0]==false)
            {
                Words[3] = Message[4];  //"Ties"
                Words[4] = Message[0];  //NOP x3
                Words[5] = Message[0];
            }
            else
            {
                Words[3] = Teams[FindHiScore(false)];  //Some team color
                Words[4] = Message[4];  //"Team"
                Words[5] = Message[5];  //"Wins"
            }
            Words[6]=Message[0];        //Pause

            for (i = 0; i <= 6; i++)   //Cycle throught words
            {
                for (j = 0; j <= 3; j++)
                {
                    Red[j].text = Words[i][0].ToString();
                    Blue[j].text = Words[i][1].ToString();

                    if (Toolbox.Options.Mode == Toolbox.mode.Battle)
                    {
                        Green[j].text = Words[i][2].ToString();
                        Yellow[j].text = Words[i][3].ToString();
                    }
                    else
                    {
                        Red[j].text = Words[i][2].ToString();
                        Blue[j].text = Words[i][3].ToString();
                    }
                }
            }

        }
        else
        {
            //Update the scores
            for (i = 1; i <= 4; i++)
            {
                Num = Score[i].ToString();
                for (j = 0; j <= 3; j++)
                {
                    switch ((Toolbox.Color)i)
                    {
                        case Toolbox.Color.Red:
                            Red[j].text = Num;
                            break;
                        case Toolbox.Color.Blue:
                            Blue[j].text = Num;
                            break;
                        case Toolbox.Color.Green:
                            Green[j].text = Num;
                            break;
                        case Toolbox.Color.Yellow:
                            Yellow[j].text = Num;
                            break;
                    }
                }
            }
        }
    }

    //Routine to update score for colored player
    //Inputs: Color of player to update, boolean determining action
    //  False=Decrement score of inputted player, increment everyone else's (for Battle mode self-score)
    //  True = Increment score of inputted player
    
    public static void UpdateScore(Toolbox.Color clr, bool action)
    {
        byte i=0;   //Generic for loop counter

        //String arrays of GameOver music
        //Aligns with Toolbox.mode enum
        string[] music =
        {
            "GO2_trsi-borntro",
            "GO1_johnmadden-scoutreport",
            "GO_scout"
        };

        Helper.light.enabled = true;    //Set the score box light to on
        
        //If incrementing a score
        if (action == true)
        {
            //If score is NOT 10, then increment
            if (Score[(byte)clr]!=10)
            {
                Score[(byte)clr]++;
            }
        }
        else
        {
            //If decrementing self-score, incrementing everyone else's
            for (i = 1; i <= 4; i++)
            {
                if (i == (byte)clr)
                {
                    //Decrement self-scorer, if score is not 0
                    if (Score[(byte)clr]!=0)
                    {
                        Score[(byte)clr]--;
                    }                    
                }
                else
                {
                    //Increment everybody else's score, if score is not 10
                    if (Score[i] != 10)
                    {
                        Score[i]++;
                    }
                }
            }
        }
               
        //If gameover condition is met (somebody has score of 10)
        //then play appropriate gameover song for mode
        if (FindHiScore(true) >= 10)
        {
            SoundManager.StopAllSounds();   //Stop all songs
            SoundManager.Play(music[(int)Toolbox.Options.Mode]);    //Play appropriate gameover song
        }
        
        SoundManager.Play("vGoal"); //"Goal!"
        SoundManager.Play("Siren"); //Siren
        ScoreTalk();    //Speak the game score
    }

    //Routine to speak the score, who is tied, and gameover conditions
    public static void ScoreTalk()
    {
        string sentence = string.Empty; //String log of sentence to speak
        byte i;        //Generic for loop counter
        byte Hi;        //The ID of highest score player OR HiScore value; usage varies in code
        bool[] HiBitField=  //Bitfield determining who is tied. 0th field is True if no ties, otherwize false
        {
            false,false,false,false,false
        };
        
        string[] words = new string[19];    //Words in sentence to speak
        //Each slot in the words array corresponds
        //to a particular usage. If not used, is NOP'd

        float[] times = new float[20];      //Time in between each word

        //Initz everything to null string words and 0.0f times
        for (i = 0; i <= 18; i++)
        {
            words[i] = string.Empty;
            times[i] = 0.0f;
        }
        
        //AudioClip strings of #s
        string[] nums=                      
        {
            "v0",
            "v1",
            "v2",
            "v3",
            "v4",
            "v5",
            "v6",
            "v7",
            "v8",
            "v9",
            "v10",
            "NULL"
        };
        //Coressponding times
        float[] numsTime=
        {
            1.5f,
            0.3f,
            0.5f,
            0.5f,
            0.6f,
            1.0f,
            0.4f,
            0.4f,
            0.3f,
            0.5f,
            0.3f,
            0.5f
        };

        //Team name AudioClip strings
        //Entries correspond to enum Colors
        string[] teams =
        {
            "NULL", //Dummy
            "vRed",
            "vGreen",
            "vBlue",
            "vYellow"
        };
        //Corresponding times
        float[] teamsTime=
        {
            0.1f,
            0.5f,
            0.5f,
            0.7f,
            0.4f
        };

        //AudioClip strings of win conditions/misc
        string[] other =
        {
            "NULL",
            "vto",
            "vTie",
            "vWins",
            "vGameover",
            "Buzzer"
        };
        //Corresponding times
        float[] otherTime =
        {
            0.1f,
            0.2f,
            0.5f,
            0.5f,
            1.5f,
            1.5f
        };

        words[0] = nums[Score[1]];  //Red Score
        words[1] = other[1];        //"to"
        //Corresponding times
        times[0] = numsTime[Score[1]];
        times[1] = otherTime[1];        

        //If NOT Battle mode, skip Green player
        if (Toolbox.Options.Mode == Toolbox.mode.Battle)
        {
            words[2] = nums[Score[2]];  //Green Score
            words[3] = other[1];        //"to"
            times[2] = numsTime[Score[2]];
            times[3] = otherTime[1];
        }
        else
        {
            words[2] = nums[11];         //nop
            words[3] = other[0];        //nop
            times[2] = numsTime[11];
            times[3] = otherTime[0];
        }

        words[4] = nums[Score[3]];  //Blue Score
        times[4] = numsTime[Score[3]];

        //If NOT Battle mode, skip Yellow Player
        if (Toolbox.Options.Mode == Toolbox.mode.Battle)
        {
            words[5] = other[1];        //"to"
            words[6] = nums[Score[4]];  //Yellow Score
            times[5] = otherTime[1];
            times[6] = numsTime[Score[4]];
        }
        else
        {
            words[5] = other[0];
            words[6] = nums[11];
            times[5] = otherTime[0];
            times[6] = numsTime[11];
        }

        Hi=FindHiScore(true);   //Find the high score # value
        FindTies(ref Score, ref HiBitField, Hi);    //Using that #, find the tie(s)
        
        //Process tie/tie win conditions
        for (i = 1; i <= 4; i++)
        {
            //If a tie...
            if (HiBitField[0] == false)
            {

                //Use bit field to announce tied teams
                if (HiBitField[i] == true)
                {
                    words[6 + i] = teams[i];    //Team color name of a tied team
                    times[6 + i] = teamsTime[i];
                }
                else
                {
                    words[6 + i] = teams[0];
                    times[6 + i] = teamsTime[0];
                }
            }

            else
            {
                words[6 + i] = teams[0];
                times[6 + i] = teamsTime[0];
            }
        }

        //If ties, then
        if (HiBitField[0]==false)
        {
            words[11] = other[2];       //"Tie"
            times[11] = otherTime[2];

            //Gameover TIE condition

            //See if the highscore is >=10 (Gameover condition)
            Hi = FindHiScore(true); //Find the highest score among the players
            if (Hi >= 10)
            {
                words[12] = other[4];       //"Gameover"
                times[12] = otherTime[4];

                words[13] = words[7];       //Say up 2 to 4 teams names that could be tied, by copying elements from tie processing
                words[14] = words[8];
                words[15] = words[9];
                words[16] = words[10];
                times[13] = times[7];
                times[14] = times[8];
                times[15] = times[9];
                times[16] = times[10];

                words[17] = other[3];   //"Wins"
                words[18] = other[5];   //Buzzer sfx
                times[17] = otherTime[3];
                times[18] = otherTime[5];
            }
            else
            {
                words[12] = other[0];
                times[12] = otherTime[0];

                words[13] = other[0];
                words[14] = other[0];
                words[15] = other[0];
                words[16] = other[0];
                times[13] = otherTime[0];
                times[14] = otherTime[0];
                times[15] = otherTime[0];
                times[16] = otherTime[0];

                words[17] = other[0];
                words[18] = other[0];
                times[17] = otherTime[0];
                times[18] = otherTime[0];
            }
        }
        else
        {
            words[11] = teams[FindHiScore(false)];  //Say currently winning team
            times[11] = teamsTime[FindHiScore(false)];

            Hi = FindHiScore(true);    //Get the highest score #
            if (Hi >= 10)
            {
                words[12] = other[4];       //"Gameover"
                words[13] = words[11];      //Some team name
                words[14] = other[0];
                words[15] = other[0];
                words[16] = other[0];
                words[17] = other[3];   //"Wins"
                words[18] = other[5];   //Buzzer sfx

                times[12] = otherTime[4];
                times[13] = times[11];
                times[14] = otherTime[0];
                times[15] = otherTime[0];
                times[16] = otherTime[0];
                times[17] = otherTime[3];
                times[18] = otherTime[5];                             
            }            
        }

        //Say each word in the sentence
        for (i = 0; i <= 18; i++)
        {            
            sentence += words[i] + " ";            
            SoundManager.Play(words[i], 1.75f);
        }

        if (Toolbox.Options.DBug == true)
        {
            Debug.Log(sentence);
        }
        Helper.light.enabled = false;   //Show the scoring light
    }

    //Routine to find the highest score value OR the index of the player with that highest score
    //Input: Boolean
    //  If true, get the highest score value; otherwise, get the index of HiScorer

    public static byte FindHiScore(bool action)
    {
        byte Hi=0;  //Byte of high score

        //Get index of highest player
        if (Score[1]>=Score[2])
        {
            if (Score[1]>=Score[3])
            {
                if (Score[1]>=Score[4])
                {
                    Hi = 1;
                }
            }
        }

        if (Score[2]>=Score[1])
        {
            if (Score[2]>=Score[3])
            {
                if (Score[2]>=Score[4])
                {
                    Hi = 2;
                }
            }
        }

        if (Score[3]>=Score[1])
        {
            if (Score[3]>=Score[2])
            {
                if (Score[3]>=Score[3])
                {
                    Hi = 3;
                }
            }
        }

        if (Score[4]>=Score[1])
        {
            if (Score[4]>=Score[2])
            {
                if (Score[4]>=Score[3])
                {
                    Hi = 4;
                }
            }
        }

        //If action==true, get the score value instead of index
        if (action == true)
        {
            Hi = Score[Hi];
        }

        return Hi;  //Return that value
    }

    //Routine to determine if there are any ties, and, if so, who is tying
    //Inputs/Outputs (by ref): Array of Scores, bitarray of who is tied, highest score value to compare for ties
    public static void FindTies(ref byte[] Scores, ref bool[] Bitfield, byte High)
    {
        byte i; //Generic FOR counter
        bool ZeroTies = true;   //Flag to determine if there are any ties. Is false if ties, otherwize, true

        //Find ties, using highest score
        for (i = 1; i <= 4; i++)
        {
            if (Scores[i]==High)
            {
                ZeroTies = false;
                Bitfield[i] = true;
            }
        }
        Bitfield[0] = ZeroTies;
    }
    

}