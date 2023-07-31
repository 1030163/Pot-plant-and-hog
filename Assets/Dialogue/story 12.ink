... #Crow
and after all that I just wanna sit here. #Potplant
huh #Crow
Forest ghosts. #Crow
Hog's leaving tommorow. #Crow
and you're trying to hold onto feelings and hold back feelings after being told you you're too much. #Crow
... #Crow
I'm sorry I hit you. #PotPlant
It's okay, it's a risk of battle. #Crow
... #Crow
You are my friend PotPlant #Crow
So if you have anything you want to talk about, #Crow
I'm here for you. #Crow
    VAR Talked = false
    VAR I_Miss_Them = false
->talk_about_it

==talk_about_it==

    { I_Miss_Them:
        *I miss them    ->I_Miss 
    } #PotPlant
     
    { Talked:
        *thank you for listening    -> END
    }
     
*I wish I could change the last thing I said to Hog #PotPlant
    ->regret
    
*How long will I feel like this #PotPlant
    ->how_Long

*It's okay to reach out to people #Crow
    ->reach_Out

    



==I_Miss==
sad
~ Talked = true
->talk_about_it

==regret==
    ~ I_Miss_Them = true
    ~ Talked = true
    ->talk_about_it
    
==how_Long==

        ~ Talked = true
    ->talk_about_it
    
==reach_Out==
    ~ Talked = true
    ->talk_about_it
