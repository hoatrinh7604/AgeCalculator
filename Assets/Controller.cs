using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;

    [SerializeField] TMP_InputField date1Year;
    [SerializeField] TMP_InputField date1Month;
    [SerializeField] TMP_InputField date1Day;

    [SerializeField] TMP_InputField date2Year;
    [SerializeField] TMP_InputField date2Month;
    [SerializeField] TMP_InputField date2Day;


    [SerializeField] GameObject groupResult;
    [SerializeField] TextMeshProUGUI resultYear;
    [SerializeField] TextMeshProUGUI resultYearSuffix;
    [SerializeField] TextMeshProUGUI resultMonth;
    [SerializeField] TextMeshProUGUI resultMonthSuffix;    
    [SerializeField] TextMeshProUGUI resultDay;
    [SerializeField] TextMeshProUGUI resultDaySuffix;

    [SerializeField] Button calculateButton;
    [SerializeField] Button currentDateButton;
    [SerializeField] Button clearButton;

    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Clear();

        currentDateButton.onClick.AddListener(delegate { GetCurrentDate(); });
        calculateButton.onClick.AddListener(delegate { Calculate(); });
        clearButton.onClick.AddListener(delegate { Clear(); });
    }

    public void OnValueChanged()
    {
        calculateButton.interactable = CheckValidate();
    }

    int year1, year2, month1, month2, day1, day2;
    private bool CheckValidate()
    {
        try
        {
            year1 = int.Parse(date1Year.text);
            year2 = int.Parse(date2Year.text);
            month1 = int.Parse(date1Month.text);
            month2 = int.Parse(date2Month.text);
            day1 = int.Parse(date1Day.text);
            day2 = int.Parse(date2Day.text);

            if(year1 <= 0 || year2 <= 0 || month1 <= 0
                || month2 <= 0 || day1 <= 0 || day2 <= 0)
                return false;

            if(year1 > year2)
            {
                if(month1 > month2)
                {
                    if(day1 > day2)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        catch(System.Exception e)
        {
            return false;
        }
    }
    public void Calculate()
    {
        SetResult2();
    }

    private void SetResult2()
    {
        groupResult.SetActive(true);

        DateTime date1 = new DateTime(year1, month1, day1);
        DateTime date2 = new DateTime(year2, month2, day2);

        int days = date2.Day- date1.Day;
        int months = date2.Month- date1.Month;
        int years = date2.Year- date1.Year;

        if (days < 0)
        {
            days = DateTime.DaysInMonth(date1.Year, date1.Month) - date1.Day +    //Days left in month of birthday +
                    date2.Day;                                                                   //Days passed in asOfDate's month
            months--;                                                                               //Subtract incomplete month that was already counted
        }

        if (months < 0)
        {
            months += 12;   //Subtract months from 12 to convert relative difference to # of months
            years--;        //Subtract incomplete year that was already counted
        }

        if (months > 0)
        {
            resultMonth.text = months.ToString();
            resultMonthSuffix.text = (months > 1) ? "Months" : "Month";
        }
        else
        {
            resultMonth.transform.parent.gameObject.SetActive(false);
        }

        // Year
        if (years > 0)
        {
            resultYear.text = years.ToString();
            resultYearSuffix.text = (years > 1) ? "Years" : "Year";
        }
        else
        {
            resultYear.transform.parent.gameObject.SetActive(false);
        }


        //Day
        if (days > 0)
        {
            resultDay.text = days.ToString();
            resultDaySuffix.text = (days > 1) ? "Days" : "Day";
        }
        else
        {
            resultDay.transform.parent.gameObject.SetActive(false);
        }
    }

    private void SetResult()
    {
        groupResult.SetActive(true);

        // Month
        int month = 0;
        if (month2 >= month1) month = month2 - month1;
        else
        {
            month = ((year2 > year1) ? 12 : 0) + month2 - month1;
        }

        if (month > 0)
        {
            resultMonth.text = month.ToString();
            resultMonthSuffix.text = (month > 1) ? "Months" : "Month";
        }
        else
        {
            resultMonth.gameObject.SetActive(false);
            resultMonthSuffix.gameObject.SetActive(false);
        }

        // Year
        int year = year2 - year1;
        if (month2 < month1) year--;
        if(year > 0 )
        {
            resultYear.text = year.ToString();
            resultYearSuffix.text = (year > 1) ? "Years" : "Year";
        }
        else
        {
            resultYear.gameObject.SetActive(false);
            resultYearSuffix.gameObject.SetActive(false);
        }


        //Day
        
        DateTime date1 = new DateTime(year1, month1, day1);
        DateTime date2 = new DateTime(year2, month2, day2);
        var span = date2.Subtract(date1);

        if (span.Days > 0)
        {
            resultDay.text = span.Days.ToString();
            resultDaySuffix.text = (span.Days > 1) ? "Days" : "Day";
        }
        else
        {
            resultDay.gameObject.SetActive(false);
            resultDaySuffix.gameObject.SetActive(false);
        }
    }

    public void GetCurrentDate()
    {
        var date = DateTime.Now;

        date2Day.text = date.Day.ToString();
        date2Month.text = date.Month.ToString();
        date2Year.text = date.Year.ToString();
    }


    public void Clear()
    {
        date1Year.text = "";
        date1Month.text = "";
        date1Day.text = "";
        date2Year.text = "";
        date2Month.text = "";
        date2Day.text = "";

        groupResult.SetActive(false);
        resultDay.text = "";
        resultMonth.text = "";
        resultYear.text = "";

        resultDaySuffix.text = "Day";
        resultMonthSuffix.text = "Month";
        resultYearSuffix.text = "Year";

        calculateButton.interactable = false;
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
