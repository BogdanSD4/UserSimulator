from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.chrome.options import Options

# Путь к исполняемому файлу ChromeDriver
chromedriver_path = 'путь_к_файлу_chromedriver'

# URL-адрес страницы Google
google_url = 'https://www.google.com'
# URL-адрес, по которой нужно перейти
target_url = 'https://vk.com/wall-45745333_46772947'
# Текст, который нужно вставить в текстовое поле
text_to_insert = 'Пример текста для вставки'

# Создаем экземпляр сервиса ChromeDriver
service = Service(chromedriver_path)

# Создаем экземпляр ChromeDriver
options = Options()
# Устанавливаем путь к папке с профилем Chrome
options.add_argument(
    f'--user-data-dir=C:/Users/dokto/AppData/Local/Google/Chrome/User Data/')
options.add_argument(f'--profile-directory=Default')
# Создаем экземпляр ChromeDriver с указанными опциями
driver = webdriver.Chrome(service=service, options=options)

try:
    # Открываем страницу Google
    driver.get(google_url)

    # Находим поле поиска на странице Google и вводим в него URL-адрес
    search_input = driver.find_element(By.NAME, 'q')
    search_input.send_keys(target_url)
    search_input.submit()

    # Ожидаем, пока ссылка станет кликабельной, и кликаем по ней
    link_locator = (By.CSS_SELECTOR, 'a[href="' + target_url + '"]')
    link = WebDriverWait(driver, 10).until(
        EC.element_to_be_clickable(link_locator))
    link.click()

    # Ожидаем, пока страница полностью загрузится
    wait = WebDriverWait(driver, 10)
    wait.until(EC.presence_of_element_located((By.TAG_NAME, 'body')))

    # Получаем текст с загруженной страницы и выводим его в консоль
    page_text = driver.find_element(By.TAG_NAME, 'body').text
    print(page_text)
except Exception as ex:
    print(f'Ошибка при загрузке страницы: {ex}')
finally:
    # Закрываем браузер и освобождаем ресурсы
    driver.quit()
