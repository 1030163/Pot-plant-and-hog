using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
    public static event Action<Story> OnCreateStory;
	
    void Awake () {
		// Remove the default message
		RemoveChildren();
		//StartStory();
	}

	// Creates a new Story object with the compiled story which we can then play!
	public void StartStory (TextAsset chapter) {
		story = new Story (chapter.text);
        if(OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}

    // This is the main function called every time the story changes. It does a few things:
    // Destroys all the old content and choices.
    // Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!

	

    private void OnMouseDown()
    {
		if (story.canContinue) {
			RefreshView();
		}
	}

	private void Update()
	{
		if (story == null)
        {
			return;
        }
		if (Input.GetKeyDown(KeyCode.Space) && story.canContinue) {
			RefreshView();
		}
	}

	public void RefreshView()
    {
		Time.timeScale = 0;
		RemoveChildren();
		CreateContentView(story.Continue());

		if (!story.canContinue && story.currentChoices.Count > 0)
		{
			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView(choice.text.Trim());
				// Tell the button what to do when we press it
				button.onClick.AddListener(delegate {
					OnClickChoiceButton(choice);
					RefreshView();
				});
			}
		}
        else if (!story.canContinue)
		{

			Button choice = CreateChoiceView("Ready?");
			choice.onClick.AddListener(delegate {
				Time.timeScale = 1;
				RemoveChildren();
			});
		}
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
		Text storyText = Instantiate (textPrefab) as Text;
		storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

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
	private Text textPrefab = null;
	[SerializeField]
	private Button buttonPrefab = null;
}
