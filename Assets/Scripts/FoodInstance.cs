using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FoodData
{
    public string foodName;
    public float foodCost;
    public string note;
    public Sprite sprite;
}

public class FoodInstance : MonoBehaviour
{
    public FoodData data;
    public Button plusBtn, minusBtn;
    public Button consigliatoPlusBtn, consigliatoMinusBtn; 
    public Text quantityText, consigliatoQuantityText;

    public int quantity;

    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            FindObjectOfType<ProductInfo>().ShowPanel(data);
        });

        plusBtn.onClick.AddListener(() =>
        {
            if (quantity == 99)
                return;

            quantity++;
            quantityText.text = quantity.ToString();

            if (consigliatoQuantityText != null)
            consigliatoQuantityText.text = quantity.ToString();

            OrderManager order = FindObjectOfType<OrderManager>();

            if (order == null)
                return;

            order.AddToList(data);
        });

        minusBtn.onClick.AddListener(() =>
        {
            if (quantity == 0)
                return;

            quantity--;
            quantityText.text = quantity.ToString();
            if (consigliatoQuantityText != null)
                consigliatoQuantityText.text = quantity.ToString();

            OrderManager order = FindObjectOfType<OrderManager>();

            if (order == null)
                return;

            order.RemoveToList(data);
        });

        if (consigliatoMinusBtn != null)
        {
            consigliatoMinusBtn.onClick.AddListener(() =>
            {
                if (quantity == 0)
                    return;

                quantity--;
                quantityText.text = quantity.ToString();

                if (consigliatoQuantityText != null)
                {
                    consigliatoQuantityText.text = quantity.ToString();

                    RefreshProductData();
                }

                OrderManager order = FindObjectOfType<OrderManager>();

                if (order == null)
                    return;

                order.RemoveToList(data);
            });
        }

        if (consigliatoPlusBtn != null)
        {
            consigliatoPlusBtn.onClick.AddListener(() =>
            {
                if (quantity == 99)
                    return;

                quantity++;
                quantityText.text = quantity.ToString();

                if (consigliatoQuantityText != null)
                {
                    consigliatoQuantityText.text = quantity.ToString();

                    RefreshProductData();
                }

                OrderManager order = FindObjectOfType<OrderManager>();

                if (order == null)
                    return;

                order.AddToList(data);
            });
        }
    }

    private void RefreshProductData()
    {
        ProductInfo info = FindObjectOfType<ProductInfo>();

        if (info != null)
        {
            if (info.productName.text == data.foodName)
            {
                info.Quantity = quantity;
                info.quantityText.text = quantity.ToString();
            }
        }
    }

    public void RefreshQuantity()
    {
        OrderManager order = FindObjectOfType<OrderManager>();

        if (order == null)
            return;

        var tempClass = order.foodList.Find(food => food.data.foodName == data.foodName);

        if (tempClass == null)
            return;

        quantity = tempClass.quantity;
        quantityText.text = quantity.ToString();

        if (consigliatoQuantityText != null)
        {
            consigliatoQuantityText.text = quantity.ToString();
        }
    }
}