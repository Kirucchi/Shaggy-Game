using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text dialogueText;
    public GameObject textBox;

    private Queue<string> sentences;

    private AudioSource audioSource;

    private void Start() {
        sentences = new Queue<string>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void StartDialogue(Dialogue dialogue) {

        textBox.SetActive(true);
        sentences.Clear();
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            StopAllCoroutines();
            textBox.SetActive(false);
            FindObjectOfType<GameManager>().StartNextPhase();
        }
        else {
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            audioSource.Play();
            yield return new WaitForSeconds(0.04f);
        }
    }

    private void Update() {
        if (textBox.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                DisplayNextSentence();
            }
        }
    }

}
