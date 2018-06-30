using UnityEngine;
using UnityEngine.UI;

public class ProductInfo : MonoBehaviour
{
    public CanvasGroup myCanvasGroup;
    public Text productName, quantityText, ingredientiText;
    public Image productImage;
    public Button plus, minus;
    public InputField inputField;

    public FoodInstance tagliata, scaloppine, vinoRosso, chianti;

    private int quantity;

    public int Quantity
    {
        get { return quantity; }
        set
        {
            quantity = value;
            inputField.enabled = quantity != 0;
        }
    }

    void Awake()
    {
        inputField.onEndEdit.AddListener(_text =>
        {
            OrderManager myOrder = FindObjectOfType<OrderManager>();

            if (myOrder == null)
                return;

            var tempClass = myOrder.foodList.Find(food => food.data.foodName == productName.text);

            if (tempClass == null)
                return;

            tempClass.data.note = _text;
        });
    }

    public void ShowPanel(FoodData _food)
    {
        Quantity = 0;

        productName.text = _food.foodName;
        productImage.sprite = _food.sprite;
        ingredientiText.text = "Ingredienti\n" + _food.foodName + " contiene......";

        OrderManager order = FindObjectOfType<OrderManager>();

        if (order != null)
        {
            OrderManager.FoodQuantityClass tempClass = order.foodList.Find(food => food.data.foodName == _food.foodName);

            Quantity = tempClass != null ? tempClass.quantity : 0;

            if (tempClass != null)
            {
                inputField.text = tempClass.data.note == string.Empty ? string.Empty : tempClass.data.note;
            }
            else
            {
                inputField.text = string.Empty;
            }
        }

        quantityText.text = Quantity.ToString();

        plus.onClick.RemoveAllListeners();

        plus.onClick.AddListener(() =>
        {
            if (Quantity == 99)
                return;

            Quantity++;
            quantityText.text = Quantity.ToString();
            
            if (order == null)
                return;

            order.AddToList(_food);

            CheckRecommendedFoods();
        });

        minus.onClick.RemoveAllListeners();

        minus.onClick.AddListener(() =>
        {
            if (Quantity == 0)
                return;

            Quantity--;
            quantityText.text = Quantity.ToString();
            
            if (order == null)
                return;

            order.RemoveToList(_food);

            CheckRecommendedFoods();
        });

        myCanvasGroup.alpha = 1;
        myCanvasGroup.blocksRaycasts = true;
        myCanvasGroup.interactable = true;
    }

    private void CheckRecommendedFoods()
    {
        switch (productName.text)
        {
            case "Tagliata di vitello":
                if (tagliata != null)
                    tagliata.RefreshQuantity();
                break;

            case "Scaloppine":
                if (scaloppine != null)
                    scaloppine.RefreshQuantity();
                break;

            case "Vino rosso intimo":
                if (vinoRosso != null)
                    vinoRosso.RefreshQuantity();
                break;

            case "Vino rosso Chianti":
                if (chianti != null)
                    chianti.RefreshQuantity();
                break;
        }
    }

    public void RefreshAllQuantities()
    {
        var foods = FindObjectsOfType<FoodInstance>();

        for (int i = 0; i < foods.Length; i++)
        {
            foods[i].RefreshQuantity();
        }
    }
}