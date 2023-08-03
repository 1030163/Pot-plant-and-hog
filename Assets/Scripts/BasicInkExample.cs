using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;
using Unity.VisualScripting;


// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
    public static event Action<Story> OnCreateStory;
	public TextAsset tutorialStory;
	public GameObject WASD;
	
    void Awake () {
		// Remove the default message
		RemoveChildren();
		//StartStory();
	}

    private void Start()
    {
		StartStory(tutorialStory);

    }


    // Creates a new Story object with the compiled story which we can then play!
    public void StartStory (TextAsset chapter) {
		story = new Story (chapter.text);
        //if(OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}


	public bool canContinueToNextLine;
	public TextMeshProUGUI storyText;
	public float typingSpeed = 0.05f;

    public IEnumerator DisplayLine(string line)
    {
        canContinueToNextLine = false;
        storyText.text = line;
        storyText = Instantiate(textPrefab) as TextMeshProUGUI;
        storyText.transform.SetParent(canvas.transform, false);
        storyText.maxVisibleCharacters = 0;

        bool isAddingRichTextTag = false;
        foreach (char letter in line.ToCharArray())
        {
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {

                storyText.text = line;
                storyText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }

        }

        canContinueToNextLine = true;
    }



	private bool loadScene = true;

    private void Update()
	{

		//Debug.Log(story.currentText);
		if (story.currentTags.Count > 0)
		{

			if (loadScene && story.currentTags[0] == "Act 2")
			{
				loadScene = false;
				Debug.Log("doing some shit");
				StartCoroutine(SceneLoad());
				return;
			}
		}

        if (story == null)
        {
			return;
        }
		if (Input.GetKeyDown(KeyCode.Space) && canContinueToNextLine) {
			RefreshView();
            return;
		}
   //     if (Input.GetKeyDown(KeyCode.Space))
   //     {
			//RemoveChildren();
			//Time.timeScale = 1;
   //     }
    }

	//public void SceneLoad(string SceneLoadInfo)
	//{
	//    SceneManager.LoadScene(SceneLoadInfo);
	//}

	public GameObject blackOut;

	public IEnumerator SceneLoad()
	{
		for (int i = 0; i < 60; i++)
		{
			blackOut.transform.position += new Vector3(1f, 0, 0);
			yield return new WaitForSecondsRealtime(0.02f);
		}
		Debug.Log("It worked?");
		SceneManager.LoadScene("Part2");
		Debug.Log("It worked");
            yield return null;
	}

    public void RefreshView()
    {
		if (story.canContinue)
        {
			Time.timeScale = 0;
			RemoveChildren();
			CreateContentView(story.Continue());
			DialogueSprite();


        }
        else if (!story.canContinue && story.currentChoices.Count > 0)
		{
			Time.timeScale = 0;
			RemoveChildren();
			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView(choice.text.Trim(), i);
				// Tell the button what to do when we press it
				button.onClick.AddListener(delegate {
					OnClickChoiceButton(choice);
					RefreshView();
				});
			}
		}
        else if (!story.canContinue)
        {
			Time.timeScale = 1;
			RemoveChildren();
			Debug.Log("WASD END OF DIALOGUE WOO");
			Instantiate(WASD, gameObject.transform);
		}
		//else if (!story.canContinue)
		//{
		//	Time.timeScale = 1;
		//	Debug.Log("WASD END OF DIALOGUE WOO");
		//	Instantiate(WASD, gameObject.transform);
		//	//Button choice = CreateChoiceView("Ready?");
		//	//choice.onClick.AddListener(delegate {
		//	//	Time.timeScale = 1;
		//	//	RemoveChildren();
		//	//});
		//}



	}

	public GameObject dialogueSpritePlaceholder;

	/*
		private void GetPlayerName()
		{
			if (story.variablesState["playerName"] == null)
			{
				Debug.Log("NO DIALOGUE SPRITE");
				return;
			}
			string dialogueSpriteName = story.variablesState["playerName"].ToString();
			Sprite dialogueSprite = Resources.Load<Sprite>(dialogueSpriteName);
			GameObject talker = Instantiate(dialogueSpritePlaceholder);
			talker.GetComponent<SpriteRenderer>().sprite = dialogueSprite; 
		}

		//Using Tags we will be better
		//A tag for the speaker and one for the emotion, a script adding them together to resource.load
		//Speaker tag can be used to colour text also hence why we seperate them
		//
	*/

	public AudioSource speaker;

	//public void PlayMusic()
	//{
	//	if (story.currentTags.Count < 3)
	//	{
	//		return;
	//	}
	//	string song = story.currentTags[2];
	//	AudioClip songAudio = Resources.Load<AudioClip>("Audio/" + song);
	//	StartCoroutine(fadeOut());
	//	speaker.clip = songAudio;
	//	speaker.Play();
	//	StartCoroutine(fadeIn());
	//}

	public void PlayMusic()
    {
		if (story.currentTags.Count < 3)
        {
			return;
        }
		string song = story.currentTags[2];
		AudioClip songAudio = Resources.Load<AudioClip>("Audio/" + song);
		StartCoroutine(fadeOut(songAudio));
    }

	public float fading;
	public bool okToQuiet;

	public IEnumerator fadeOut(AudioClip songAudio)
    {

		while (speaker.volume > 0f)
        {
			okToQuiet = false;
			//Debug.Log("AAHHH " + speaker.volume);
			speaker.volume -= fading;
			yield return new WaitForSecondsRealtime(0.02f);
		}
		okToQuiet = true;
		speaker.clip = songAudio;
        speaker.Play();
        StartCoroutine(fadeIn());
        yield return null;
    }

	public IEnumerator fadeIn()
	{
		while (speaker.volume < 1 && okToQuiet)
		{
			speaker.volume += fading;
			yield return new WaitForSecondsRealtime(0.02f);
		}
		yield return null;
	}

	public void DialogueSprite()
	{
		PlayMusic();



		string character = null;
		string mood = null;

		if (story.currentTags.Count > 0)
        {
			character = story.currentTags[0];
		}
		if (story.currentTags.Count > 1)
		{
			mood = story.currentTags[1];
		}
		Debug.Log("Ignore error");

		if (mood == null)
        {
			mood = "Default";
        }
		//finds the sprite based on the mood and character
		// PotPlant/Sad, PotPlant/Happy, Hog/Happy
		string dialogueSpriteName = "DialogueSprites/" + character + "/" + mood;
		
		
        Sprite dialogueSprite = Resources.Load<Sprite>(dialogueSpriteName);

		if (dialogueSprite == null)
		{
			Debug.Log(dialogueSpriteName + " sprite does not exist");
			dialogueSprite = Resources.Load<Sprite>("DialogueSprites/" + character + "/Default");
		}

        //prepares wiggle prefab
        //Wiggle_Sad, Wiggle_Nervous etc.


        dialogueSpritePlaceholder = Resources.Load<GameObject>("DialogueSprites/Wiggle/" + mood);
		if (dialogueSpritePlaceholder == null)
		{
			Debug.Log(dialogueSpriteName + " Mood does not exist");
			dialogueSpritePlaceholder = Resources.Load<GameObject>("DialogueSprites/Wiggle/Default");
		}
		GameObject talker = Instantiate(dialogueSpritePlaceholder);

		//Assigns sprite to wiggle prefab
        talker.GetComponent<SpriteRenderer>().sprite = dialogueSprite;

		//Adds to UI canvas (also allowing it to be removed later)
        talker.transform.SetParent(canvas.transform, false);

    }


	/*
    void RefreshView () {
		// Remove all the UI on screen
		RemoveChildren ();
		
		//// Read all the content until we can't continue any more
		//if (story.canContinue) {
		//	// Continue gets the next line of the story
		//	string text = story.Continue ();
		//	// This removes any white space from the text.
		//	text = text.Trim();
		//	// Display the text on screen!
		//	CreateContentView(text);
		//}

		// Display all the choices, if there are any!

		if (story.canContinue)
        {
			Button progress = CreateChoiceView(story.Continue());
			progress.onClick.AddListener(delegate {
				RefreshView();
				//string text = story.Continue();
				// This removes any white space from the text.
				//text = text.Trim();
				// Display the text on screen!
				//CreateContentView(text);
			});
		}
		if(story.currentChoices.Count > 0 && !story.canContinue) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
					RefreshView();
					string text = story.Continue();
					// This removes any white space from the text.
					text = text.Trim();
					// Display the text on screen!
					CreateContentView(text);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else if (!story.canContinue) {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}
        else
        {
			
		}
	}
	*/

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {
        StartCoroutine(DisplayLine(text));
    }

    // Creates a button showing the choice text		
    Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate(buttonPrefab[0]) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		//HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		//layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	public int buttonSplit;
    Button CreateChoiceView(string text, int i)
    {
        // Creates the button from a prefab
        Button choice = Instantiate(buttonPrefab[i]) as Button;
        choice.transform.SetParent(canvas.transform, false);

        // Gets the text from the button prefab
        Text choiceText = choice.GetComponentInChildren<Text>();
        choiceText.text = text;

        // Make the button expand to fit the text
        //HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
        //layoutGroup.childForceExpandHeight = false;

        return choice;
    }

    // Destroys all the children of this gameobject (all the UI)
    void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

	[SerializeField]
	//private TextAsset inkJSONAsset = null;
	public Story story;

	[SerializeField]
	private Canvas canvas = null;

	// UI Prefabs
	[SerializeField]
	private TextMeshProUGUI textPrefab = null;
	[SerializeField]
	public Button[] buttonPrefab = null;
}
