using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private enum MenuState
    {
        INTRO,
        INACTIVE,
        ACTIVE
    }

    [SerializeField] private Canvas main_menu_canvas;
    [SerializeField] private Canvas credits_canvas;
    [SerializeField] private ScreenFadeEffect screen_fade;
    [SerializeField] private IntroCutscene intro_cutscene;
    [SerializeField] private Image selection_highlight;
    [SerializeField] private UIButtonEvents[] buttons;
    [SerializeField] [Range(0f, 1f)] private float lerp_speed;
    private MenuState menu_state;
    private RectTransform target_button;
    private Coroutine fade_routine;
    private Coroutine highlight_routine;

    private void Start()
    {
        // Disable all canvases at start
        if (main_menu_canvas != null) { main_menu_canvas.gameObject.SetActive(false); }
        if (credits_canvas != null) { credits_canvas.gameObject.SetActive(false); }

        if (screen_fade != null)
        {
            fade_routine = StartCoroutine(screen_fade.FadeRoutine(Color.black, new Color(0f, 0f, 0f, 0f), 3.5f));
        }

        if (intro_cutscene != null)
        {
            intro_cutscene.CutsceneIsOver += IntroCutscene_CutsceneEnded;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SubscribeToMouseOverEvent(UIButtonEvents_OnHover);
        }

        menu_state = MenuState.INTRO;
        StartCoroutine(CheckForSkipIntroRoutine());
    }

    private void UIButtonEvents_OnHover(UIButtonEvents sender)
    {
        RectTransform rt = sender.transform as RectTransform;

        if (rt != target_button && menu_state == MenuState.ACTIVE)
        {
            selection_highlight.gameObject.SetActive(true);
            target_button = rt;

            if (highlight_routine != null)
            {
                StopCoroutine(highlight_routine);
            }

            highlight_routine = StartCoroutine(HighlightRoutine());
        }
    }

    private void IntroCutscene_CutsceneEnded()
    {
        menu_state = MenuState.INACTIVE;
        intro_cutscene.CutsceneIsOver -= IntroCutscene_CutsceneEnded;
    }

    public void StartGame_ButtonPressed()
    {
        menu_state = MenuState.INACTIVE;
        StartCoroutine(StartGameRoutine());
    }

    public void Credits_ButtonPressed()
    {
        menu_state = MenuState.INACTIVE;
        main_menu_canvas.gameObject.SetActive(false);
        credits_canvas.gameObject.SetActive(true);
    }

    public void Credits_ReturnButtonPressed()
    {
        menu_state = MenuState.ACTIVE;
        main_menu_canvas.gameObject.SetActive(true);
        credits_canvas.gameObject.SetActive(false);
    }

    public void QuitGame_ButtonPressed()
    {
        Application.Quit();
    }

    private IEnumerator CheckForSkipIntroRoutine()
    {
        while (menu_state == MenuState.INTRO)
        {
            if (Input.GetMouseButtonDown(0))
            {
                menu_state = MenuState.INACTIVE;
                intro_cutscene.StopAnimation();

                // If we skip the cutscene during the initial fade-in transition, stop it before continuting
                if (fade_routine != null)
                {
                    StopCoroutine(fade_routine);
                }

                yield return screen_fade.FadeRoutine(screen_fade.FadeObjectColor, Color.white, 0.5f); // Fade to white
                intro_cutscene.PlayAnimation("MainMenu"); // Move the camera to its final position
                yield return screen_fade.FadeRoutine(screen_fade.FadeObjectColor, new Color(1f, 1f, 1f, 0f), 1.5f); // Fade to transparent
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        menu_state = MenuState.ACTIVE;
        main_menu_canvas.gameObject.SetActive(true);
    }

    private IEnumerator HighlightRoutine()
    {
        while (Vector3.Distance(selection_highlight.rectTransform.position, target_button.position) > 0.001f)
        {
            selection_highlight.rectTransform.position = Vector3.Lerp(selection_highlight.rectTransform.position, target_button.position, lerp_speed);
            yield return null;
        }
    }

    private IEnumerator StartGameRoutine()
    {
        if (screen_fade != null)
        {
            yield return screen_fade.FadeRoutine(new Color(0f, 0f, 0f, 0f), Color.black, 2f);
            yield return new WaitForSeconds(0.5f);
        }
        else
            yield return null;

        SceneManager.LoadScene("BensTestScene");
    }
}
