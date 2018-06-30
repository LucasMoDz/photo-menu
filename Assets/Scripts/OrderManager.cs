using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public List<FoodQuantityClass> foodList;

    public Button orderBtn;
    public Text orderText;

    public void Awake()
    {
        foodList = new List<FoodQuantityClass>();
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

        FoodData newData = new FoodData();
        newData.foodName = _food.foodName;
        newData.sprite = _food.sprite;
        newData.foodCost = _food.foodCost;

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
}