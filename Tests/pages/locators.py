from selenium.webdriver.common.by import By


class BasePageLocators:
    MAIN_ICON = (By.CSS_SELECTOR, "#app > div > header > nav > div > img")


class AdsPageLocators:
    ADS_TABLE = (By.CSS_SELECTOR, "#app > div > div > main > div:nth-child(1) > div.react-bootstrap-table")
