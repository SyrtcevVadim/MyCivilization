using UnityEngine;
/* Камера может перемещаться только внутри игровой области, которая ограничена границами:
 * leftBorder - левая граница.
 * rightBorder - правая граница.
 * topBorder - верхняя граница
 * bottomBorder - нижняя граница.
 * 
 * Передвижение камеры осуществляется нажатием кнопок W,A,S,D:
 * W: переместить камеру вверх.
 * A: переместить камеру влево.
 * S: переместить камеру вниз.
 * D: переместить камеру вправо.
 * 
 * При попытке игрока пересечь границы игровой области камера переносится на противоположную границу.
 * Таким образом моделируется "облетания Земного шара"
 */
/// <summary>
/// Контроллирует передвижения камеры нажатием кнопок W,A,S,D.
/// </summary>
public class CameraMoving : MonoBehaviour
{
    /// <summary>
    /// Скорость передвижения камеры. Экспериментально было выяснено, что комфортного перемещения камеры можно добиться, 
    /// используя значение 0.3f
    /// </summary>
    [SerializeField] private float movingSpeed = 0.3f;
    /// <summary>
    /// Главная камера
    /// </summary>
    Camera mainCamera;

    // Границы игровой зоны.
    // Запрещается перемещать камеру вне игрового поля.
    /// <summary>
    /// Левая граница игрового поля
    /// </summary>
    private GameObject leftBorder;
    /// <summary>
    /// Правая граница игрового поля
    /// </summary>
    private GameObject rightBorder;
    /// <summary>
    /// Верхняя граница игрового поля
    /// </summary>
    private GameObject topBorder;
    /// <summary>
    /// Нижняя граница игрового поля
    /// </summary>
    private GameObject bottomBorder;

    private void Awake()
    {
        // Получаем границы игрового поля
        leftBorder = GameObject.Find("LeftBorder");
        rightBorder = GameObject.Find("RightBorder");
        topBorder = GameObject.Find("TopBorder");
        bottomBorder = GameObject.Find("BottomBorder");
        // Получаем камеру, передвижения которой контроллируются.
        mainCamera = GetComponent<Camera>();
    }
    private void FixedUpdate()
    {
        // Обработка передвижения камеры осуществляется, если окно внутриигрового меню неактивно
        if(!MenuPanelLogic.IsMenuActive())
        {
            ProcessCameraMovement();
        }
        
    }

    /// <summary>
    /// Проверяет, находится ли камера внутри игровой зоны. Игровая зона ограничивается границами.
    /// </summary>
    /// <returns>true, если камера находится внутри игровой зоны. Иначе - false.</returns>
    bool IsCameraInsidePlayField()
    {
        bool first = transform.position.x <= rightBorder.transform.position.x;
        bool second = leftBorder.transform.position.x <= transform.position.x;
        bool third = transform.position.y <= topBorder.transform.position.y;
        bool forth = bottomBorder.transform.position.y <= transform.position.y;
        if(first&&second&third&&forth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Обрабатывает перемещение камера в двумерной плоскости
    /// </summary>
    private void ProcessCameraMovement()
    { 
        float horizontalMoving = Input.GetAxis("Horizontal");   // Получаем вектор перемещения по оси OX
        float verticalMoving = Input.GetAxis("Vertical");       // Получаем вектор перемещения по оси OY
        Vector3 movement = new Vector2(horizontalMoving, verticalMoving);   // Результирующее перемещение камеры(его еще нужно умножить на скорость перемещения)
        Vector3 newCameraPosition = Camera.main.transform.position + movement;  // Новая возможная позиция камеры
        if(IsCameraInsidePlayField())
        {
            // Если камера находится внутри игровой области, то разрешаем ее перемещение в штатном режиме
            transform.Translate(movement * movingSpeed);
        }
        else
        {
            // Если камера пересекла левую границу, перемещаем ее на правую границу
            if(transform.position.x < leftBorder.transform.position.x)
            {
                transform.position = new Vector3(rightBorder.transform.position.x, transform.position.y, -10f);
            }
            // Если камера пересекла правую границу, перемещаем ее на левую границу
            else if(rightBorder.transform.position.x < transform.position.x)
            {
                transform.position = new Vector3(leftBorder.transform.position.x, transform.position.y, -10f);
            }
            // Если камера пересекла верхнюю границу, перемещаем ее на нижнюю границу
            else if(transform.position.y > topBorder.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, bottomBorder.transform.position.y, -10f);
            }
            // Если камера пересекла нижнюю границу, перемещаем ее на верхнюю границу
            else if(transform.position.y < bottomBorder.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, topBorder.transform.position.y, -10f);
            }
        }
    }
}
