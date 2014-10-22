//Behavior to attach/process physical goal objects

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class GoalBeh : MonoBehaviour {

    GameObject Goal;            //GameObject of this Goal object
    GameObject ScoreHelper;     //GameObject of invis Scoring thinker object
    List<Tags> GoalTags;        //The tags on the goal    
	// Use this for initialization
	void Start () {
        //Get the goal object and its tags
        Goal = this.gameObject;
        GoalTags = Goal.tagsEnum();
        Toolbox.Options.Mode = Toolbox.mode.Battle; //@Force mode to Battle Mode, for debug testing. Remove me!
	}

    //Process goal collisions
    void OnCollisionEnter(Collision other)    {

        GameObject otherObj = other.gameObject; //Get the tags of the collided object
        List<Tags> OtherTags = otherObj.gameObject.tagsEnum();  //Get its tags
        Tags PuckColor; //Get the puck color
        Tags GoalColor; //Get the goal color

        //Check collision with pucks
        if (OtherTags.Contains(Tags.Puck)==true)
        {
            //@otherObj.AddTag(Tags.Reset);        //Respawn puck command
            GoalColor = Toolbox.Tagmgmt.GetObjColorTag(Goal);        //Get goal color
            PuckColor = Toolbox.Tagmgmt.GetObjColorTag(otherObj);  //Get puck's color

            //Check for a self-goal
            if (PuckColor == GoalColor)
            {
                if (Toolbox.Options.Mode == Toolbox.mode.Battle)
                {
                    //If Battle, do special goal
                    Scoring.UpdateScore(Toolbox.Tagmgmt.Tag2Color(PuckColor), false);
                }
                else
                {
                    //If a 2p mode, ALWAYS give point to opposite colored goal
                    if (GoalColor == Tags.Blue)
                    {
                        Scoring.UpdateScore(Toolbox.Color.Red, true);
                    }
                    else
                    {
                        Scoring.UpdateScore(Toolbox.Color.Blue, true);
                    }
                }
            }
            else
            {
                //Do normal goal
                Scoring.UpdateScore(Toolbox.Tagmgmt.Tag2Color(PuckColor), true);
            }
        }        
    }
}
