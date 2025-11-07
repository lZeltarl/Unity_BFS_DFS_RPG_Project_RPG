using UnityEngine;
using UnityEngine.UI;

// UIController: liga botões da cena aos métodos do SearchController.
// Crie um Canvas com três botões e arraste-os a este componente.
public class UIController : MonoBehaviour
{
    public Button bfsButton;
    public Button dfsButton;
    public Button resetButton;
    public SearchController searchController;

    void Start()
    {
        if (bfsButton!=null) bfsButton.onClick.AddListener(() => OnBFSClicked());
        if (dfsButton!=null) dfsButton.onClick.AddListener(() => OnDFSClicked());
        if (resetButton!=null) resetButton.onClick.AddListener(() => OnResetClicked());
    }

    void OnBFSClicked()
    {
        if (searchController==null) return;
        searchController.ResetGraph();
        searchController.RunBFS();
    }

    void OnDFSClicked()
    {
        if (searchController==null) return;
        searchController.ResetGraph();
        searchController.RunDFS();
    }

    void OnResetClicked()
    {
        if (searchController==null) return;
        searchController.ResetGraph();
    }
}
