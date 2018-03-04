using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Leap;

public class CalcScript : MonoBehaviour {

    public InputField inputField;
    public GameObject Palm;
    public Color StandartColor;
    public Color NewColor;

    public Vector3 OldPosition;
    public GameObject CurrentButton;
    public Vector3 bias;

    public void SetNewScale(GameObject button)
    {
        if (CurrentButton == null)
        {
            OldPosition = button.transform.localPosition;
            CurrentButton = button;
            StandartColor = CurrentButton.gameObject.transform.Find("ButtonBackdrop").gameObject.GetComponent<UnityEngine.UI.Image>().color;
            button.gameObject.transform.Find("ButtonBackdrop").gameObject.GetComponent<UnityEngine.UI.Image>().color = NewColor;

            button.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1f);

            Debug.Log("Button position was : "+ button.GetComponent<RectTransform>().localPosition);

            button.GetComponent<RectTransform>().localPosition = new Vector3(
                button.transform.localPosition.x, 
                button.transform.localPosition.y, 
                -30);
        }

    }

    public void StandartScale(GameObject button)
    {
        if (CurrentButton != null)
            if (CurrentButton.GetComponentInChildren<Text>().text.Equals(button.GetComponentInChildren<Text>().text))
            {
                button.gameObject.transform.Find("ButtonBackdrop").gameObject.GetComponent<UnityEngine.UI.Image>().color = StandartColor;
                CurrentButton = null;
                button.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                button.GetComponent<RectTransform>().localPosition = OldPosition;
                //button.GetComponent<RectTransform>().position = new Vector3(button.transform.position.x, button.transform.position.y, 0.1f);
                Debug.Log("Button position now : " + button.GetComponent<RectTransform>().localPosition.x);
            }
    }

    public void setTextToInputField()
    {

        String txt = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;

        if (inputField.text.Equals("Invalid expression"))
            inputField.text = "0";

        switch (txt)
        {
            case "=":
                if (!(inputField.text[inputField.text.Length - 1].Equals('*')) &&
                    !(inputField.text[inputField.text.Length - 1].Equals('+')) &&
                    !(inputField.text[inputField.text.Length - 1].Equals('-')) &&
                    !(inputField.text[inputField.text.Length - 1].Equals('/')))
                    try
                    {
                        inputField.text = RPN.Calculate(inputField.GetComponent<InputField>().text).ToString();
                    }
                    catch
                    {
                        inputField.text = "Invalid expression";
                    }
                break;

            case "C":
                inputField.text = "0";
                break;

            case "<-":
                if(inputField.text.Length != 1)
                    inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
                else
                    inputField.text = "0";
                break;

            case ".":
                if (inputField.text.Equals(""))
                    inputField.text = "0.";
                else
                    inputField.text = inputField.text + txt;
                break;
            
            case "(" :
                if (inputField.text.Length != 0 && !inputField.text.Equals("0"))
                {
                    if (!(inputField.text[inputField.text.Length - 1].Equals('*') ||
                        inputField.text[inputField.text.Length - 1].Equals('-') ||
                        inputField.text[inputField.text.Length - 1].Equals('+') ||
                        inputField.text[inputField.text.Length - 1].Equals('/')))
                    {

                        if (inputField.text[inputField.text.Length - 1].Equals('.'))
                            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1) + "*(";
                        else if (inputField.text[inputField.text.Length - 1].Equals('('))
                            inputField.text = inputField.text + "(";
                        else
                            inputField.text = inputField.text + "*(";
                    }

                    else
                        inputField.text = inputField.text + "(";
                }
                else
                    inputField.text = "(";

                break;

            case ")" :
                if (inputField.text.Length != 0)

                    if (!(inputField.text[inputField.text.Length - 1].Equals('*') ||
                        inputField.text[inputField.text.Length - 1].Equals('-') ||
                        inputField.text[inputField.text.Length - 1].Equals('+') ||
                        inputField.text[inputField.text.Length - 1].Equals('/') ||
                        inputField.text[inputField.text.Length - 1].Equals('.') ||
                        inputField.text[inputField.text.Length - 1].Equals('(')))
                    {
                        inputField.text = inputField.text + ")";
                    }
                    else if (inputField.text[inputField.text.Length - 1].Equals('('))
                        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
                    else
                        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1) + ")";

                break;

            default:
                if (!(inputField.text.Equals("0") || inputField.text.Equals("")))
                    if (txt.Equals("*") || txt.Equals("/") || txt.Equals("+") || txt.Equals("-"))
                    {

                        if (inputField.text[inputField.text.Length-1].Equals('*')||
                        inputField.text[inputField.text.Length-1].Equals('-') ||
                        inputField.text[inputField.text.Length-1].Equals('+') ||
                        inputField.text[inputField.text.Length-1].Equals('/'))
                        {
                            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1) + txt;
                        }else
                            inputField.text = inputField.text + txt;

                    }else if (inputField.text[inputField.text.Length - 1].Equals(')'))
                        inputField.text = inputField.text +"*"+ txt;
                    else
                        inputField.text = inputField.text + txt;

                else
                    inputField.text = txt;

                break;

        }
            
    }
}


class RPN
{
    //Метод Calculate принимает выражение в виде строки и возвращает результат, в своей работе использует другие методы класса
    static public double Calculate(string input)
    {
        string output = GetExpression(input); //Преобразовываем выражение в постфиксную запись
        double result = Counting(output); //Решаем полученное выражение
        return result; //Возвращаем результат
    }

        //Метод, преобразующий входную строку с выражением в постфиксную запись
    static private string GetExpression(string input)
    {
        string output = string.Empty; //Строка для хранения выражения
        Stack<char> operStack = new Stack<char>(); //Стек для хранения операторов

        for (int i = 0; i < input.Length; i++) //Для каждого символа в входной строке
        {
            //Разделители пропускаем
            if (IsDelimeter(input[i]))
                continue; //Переходим к следующему символу

            //Если символ - цифра, то считываем все число
            if (char.IsDigit(input[i])) //Если цифра
            {
                //Читаем до разделителя или оператора, что бы получить число
                while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                {
                    output += input[i]; //Добавляем каждую цифру числа к нашей строке
                    i++; //Переходим к следующему символу

                    if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                }

                output += " "; //Дописываем после числа пробел в строку с выражением
                i--; //Возвращаемся на один символ назад, к символу перед разделителем
            }

            //Если символ - оператор
            if (IsOperator(input[i])) //Если оператор
            {
                if (input[i] == '(') //Если символ - открывающая скобка
                    operStack.Push(input[i]); //Записываем её в стек
                else if (input[i] == ')') //Если символ - закрывающая скобка
                {
                    //Выписываем все операторы до открывающей скобки в строку
                    char s = operStack.Pop();

                    while (s != '(')
                    {
                        output += s.ToString() + ' ';
                        s = operStack.Pop();
                    }
                }
                else //Если любой другой оператор
                {
                    if (operStack.Count > 0) //Если в стеке есть элементы
                        if (GetPriority(input[i]) <= GetPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                            output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                    operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека

                }
            }
        }

        //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
        while (operStack.Count > 0)
            output += operStack.Pop() + " ";

        return output; //Возвращаем выражение в постфиксной записи
    }

    //Метод, вычисляющий значение выражения, уже преобразованного в постфиксную запись
    static private double Counting(string input)
    {
        double result = 0; //Результат
        Stack<double> temp = new Stack<double>(); //Dhtvtyysq стек для решения

        for (int i = 0; i < input.Length; i++) //Для каждого символа в строке
        {
            //Если символ - цифра, то читаем все число и записываем на вершину стека
            if (char.IsDigit(input[i]))
            {
                string a = string.Empty;

                while (!IsDelimeter(input[i]) && !IsOperator(input[i])) //Пока не разделитель
                {
                    a += input[i]; //Добавляем
                    i++;
                    if (i == input.Length) break;
                }
                temp.Push(double.Parse(a)); //Записываем в стек
                i--;
            }
            else if (IsOperator(input[i])) //Если символ - оператор
            {
                //Берем два последних значения из стека
                double a = temp.Pop();
                double b = temp.Pop();

                switch (input[i]) //И производим над ними действие, согласно оператору
                {
                    case '+': result = b + a; break;
                    case '-': result = b - a; break;
                    case '*': result = b * a; break;
                    case '/': result = b / a; break;
                    case '^': result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                }
                temp.Push(result); //Результат вычисления записываем обратно в стек
            }
        }
        return temp.Peek(); //Забираем результат всех вычислений из стека и возвращаем его}
    }

        //Метод возвращает true, если проверяемый символ - разделитель ("пробел" или "равно")
    static private bool IsDelimeter(char c)
    {
        if ((" =".IndexOf(c) != -1))
            return true;
        return false;
    }

    //Метод возвращает true, если проверяемый символ - оператор
    static private bool IsOperator(char с)
    {
        if (("+-/*^()".IndexOf(с) != -1))
            return true;
        return false;
    }

    //Метод возвращает приоритет оператора
    static private byte GetPriority(char s)
    {
        switch (s)
        {
            case '(': return 0;
            case ')': return 1;
            case '+': return 2;
            case '-': return 3;
            case '*': return 4;
            case '/': return 4;
            case '^': return 5;
            default: return 6;
        }
    }
}