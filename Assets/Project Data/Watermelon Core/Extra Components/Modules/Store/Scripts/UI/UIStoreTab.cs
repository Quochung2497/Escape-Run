using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon.Store
{
    [RequireComponent(typeof(Button))]
    public class UIStoreTab : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Image enabledMask;
        [SerializeField] Image disabledMask;
        [SerializeField] Image backImage;

        public TabData Data { get; private set; }
        public bool IsSelected { get; private set; }

        private TabData.SimpleTabDelegate onTabSelected;

        public void Init(TabData data,TabData.SimpleTabDelegate onTabSelected)
        {
            Data = data;
            this.onTabSelected = onTabSelected;

            text.text = Data.Name;
            backImage.color = data.BackgroundColor;
        }

        public void SetSelectedStatus(bool isSelected)
        {
            IsSelected = isSelected;

            enabledMask.enabled = isSelected;
            disabledMask.enabled = !isSelected;

            button.enabled = !isSelected;
        }

        public void OnButtonClick()
        {
            onTabSelected?.Invoke(Data);
        }
    }
}