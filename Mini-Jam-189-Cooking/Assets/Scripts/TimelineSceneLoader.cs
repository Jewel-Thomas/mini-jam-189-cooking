using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineSceneLoader : MonoBehaviour
{
    public PlayableDirector timeline;
    public int nextSceneIndex;

    void Start()
    {
        if (timeline == null)
            timeline = GetComponent<PlayableDirector>();
        if (timeline != null)
            timeline.stopped += OnTimelineStopped;
    }

    void OnDestroy()
    {
        if (timeline != null)
            timeline.stopped -= OnTimelineStopped;
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
}
