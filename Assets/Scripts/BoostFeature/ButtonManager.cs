using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.BoostFeature
{
    public class ButtonManager : MonoBehaviour
    {
        public Action<Categories> onCategoryChanged;

        [SerializeField] public Categories currentCategory;
        
        [SerializeField] private List<Button> buttons;
        [SerializeField] private RectTransform contentView;
        
        private void Start()
        {
            foreach (var button in this.buttons)
            {
                button.onClick.AddListener(() => OnClickActive(button.GetComponent<CategoryButton>().categoryButton));
            }
        }

        public void Restart()
        {
            foreach (var button in this.buttons)
            {
                if (buttons[0] == button)
                {
                    button.interactable = false;
                    button.gameObject.transform.SetAsFirstSibling();
                    this.contentView.position = new Vector3(0, this.contentView.position.y, this.contentView.position.z);
                    continue;
                }

                button.interactable = true;
            }
        }

        public void OnClick(Button clickedButton)
        {
            foreach (var button in this.buttons)
            {
                button.interactable = button != clickedButton;
            }
            
            clickedButton.gameObject.transform.SetAsFirstSibling();
            this.contentView.position = new Vector3(0, this.contentView.position.y, this.contentView.position.z);
        }

        private void OnClickActive(Categories category)
        {
            this.currentCategory = category;
            this.onCategoryChanged?.Invoke(category);
        }
    }
}