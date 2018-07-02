using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public List<FoodQuantityClass> foodList;

    public Transform parentFood;
    public CanvasGroup panel;

    public GameObject foodPrefab, totPrefab;

    public Button orderBtn;
    public Text orderText;

    public void Awake()
    {
        foodList = new List<FoodQuantityClass>();

        panel.alpha = 0;
        panel.blocksRaycasts = false;
        panel.interactable = false;

        orderBtn.onClick.AddListener(ShowPanel);
    }

    [Serializable]
    public class FoodQuantityClass
    {
        public FoodData data;
        public int quantity;

        public FoodQuantityClass(FoodData _data, int _quantity)
        {
            data = _data;
            quantity = _quantity;
        }
    }

    public void AddToList(FoodData _food)
    {
        FoodQuantityClass tempClass = foodList.Find(food => food.data.foodName.Equals(_food.foodName));

        FoodData newData = new FoodData
        {
            foodName = _food.foodName,
            sprite = _food.sprite,
            foodCost = _food.foodCost
        };

        if (tempClass == null)
        {
            foodList.Add(new FoodQuantityClass(newData, 1));
        }
        else
        {
            tempClass.quantity++;
        }

        orderText.text = "Ordine (" + GetOrderLength() + ")";
    }

    public void RemoveToList(FoodData _food)
    {
        FoodQuantityClass tempClass = foodList.Find(food => food.data.foodName.Equals(_food.foodName));

        if (tempClass == null)
        {
            return;
        }

        tempClass.quantity --;

        if (tempClass.quantity == 0)
        {
            foodList.Remove(tempClass);
        }

        if (foodList.Count == 0)
        {
            foodList.Clear();
            foodList.TrimExcess();
        }

        orderText.text = "Ordine (" + GetOrderLength() + ")";
    }

    private string GetOrderLength()
    {
        int number = 0;

        for (int i = 0; i < foodList.Count; i++)
        {
            number += foodList[i].quantity;
        }

        return number.ToString();
    }

    public void ShowPanel()
    {
        for (int i = 0; i < foodList.Count; i++)
        {
            GameObject newGo = Instantiate(foodPrefab.gameObject, parentFood);
            RepoSingleOrder repo = newGo.GetComponent<RepoSingleOrder>();

            if (repo == null)
                continue;

            repo.productName.text = foodList[i].data.foodName;
            repo.productCost.text = (foodList[i].data.foodCost * foodList[i].quantity).ToString("##.#0") + " €";
            repo.quantity.text = foodList[i].quantity.ToString(CultureInfo.InvariantCulture);

            if (foodList[i].data.note != string.Empty)
            {
                repo.note.text = foodList[i].data.note;
            }

            repo.myImage.sprite = foodList[i].data.sprite;

            string temp_foodName = foodList[i].data.foodName;

            repo.plusBtn.onClick.RemoveAllListeners();
            repo.minusBtn.onClick.RemoveAllListeners();

            repo.note.onEndEdit.AddListener(_note =>
            {
                var food = foodList.Find(_food => _food.data.foodName == temp_foodName);

                if (food == null)
                    return;

                food.data.note = _note;
            });

            repo.plusBtn.onClick.AddListener(() =>
            {
                var food = foodList.Find(_food => _food.data.foodName == temp_foodName);
                
                if (food.quantity == 99)
                    return;

                food.quantity++;
                repo.quantity.text = food.quantity.ToString();

                repo.productCost.text = (food.data.foodCost * food.quantity).ToString("##.#0") + " €";

                RefreshTotalCost();
            });

            repo.minusBtn.onClick.AddListener(() =>
            {
                var food = foodList.Find(_food => _food.data.foodName == temp_foodName);

                if (food.quantity == 0)
                    return;

                food.quantity--;
                repo.quantity.text = food.quantity.ToString();

                if (food.quantity == 0)
                {
                    DestroyImmediate(newGo);
                }
                else
                {
                    repo.productCost.text = (food.data.foodCost * food.quantity).ToString("##.#0") + " €";
                }

                RefreshTotalCost();
            });
        }
       
        totPrefab.transform.SetAsLastSibling();

        RefreshTotalCost();

        panel.alpha = 1;
        panel.blocksRaycasts = true;
        panel.interactable = true;
    }

    private void RefreshTotalCost()
    {
        float tot = 0f;

        for (int i = 0; i < foodList.Count; i++)
        {
            tot += foodList[i].data.foodCost * foodList[i].quantity;
        }

        Text totText = GameObject.FindGameObjectWithTag("Tot").GetComponent<Text>();
        totText.text = (Mathf.Approximately(0.0f, tot) ? "0.00" : tot.ToString("##.#0")) + " €";
    }

    public void ClearOrderList()
    {
        for (int i = 0; i < parentFood.childCount; i++)
        {
            if (parentFood.GetChild(i).GetChild(0).CompareTag("Tot"))
                continue;

            Destroy(parentFood.GetChild(i).gameObject);
        }

        for (int i = foodList.Count - 1; i > 0; i--)
        {
            if (foodList[i].quantity > 0)
                continue;

            foodList.RemoveAt(i);
        }

        var panels = FindObjectsOfType<CanvasGroup>();

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].alpha = 0;
            panels[i].interactable = false;
            panels[i].blocksRaycasts = false;
        }

        orderText.text = "Ordine (" + GetOrderLength() + ")";

        FindObjectOfType<ProductInfo>().RefreshAllQuantities();
    }
}