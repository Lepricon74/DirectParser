from selenium.webdriver.common.by import By


class BasePageLocators:
    MAIN_ICON = (By.CSS_SELECTOR, "#app > div > header > nav > div > img")


class AdsPageLocators:
    ADS_TABLE = (By.CSS_SELECTOR, "#app > div > div > main > div:nth-child(1) > div.react-bootstrap-table")
    FILTER = (By.ID, "filter-container")
    END_DATE = (By.ID, "end-date")
    START_DATE = (By.ID, "start-date")
    FILTER_SELECT = (By.ID, "filter-select")
    FILTER_BUTTON = (By.ID, "filter-ads")
    ROWS = (By.XPATH, './/tr')
    FIRST_TBODY = (By.XPATH, '//*[@id="app"]/div/div/main/div[1]/div[2]/table/tbody')
    FIRST_ROW_DATE = (By.CSS_SELECTOR,
                      "#app > div > div > main > div:nth-child(1) > div.react-bootstrap-table > table > tbody > tr:nth-child(1) > td:nth-child(5)")
    SECOND_ROW_DATE = (By.CSS_SELECTOR,
                      "#app > div > div > main > div:nth-child(1) > div.react-bootstrap-table > table > tbody > tr:nth-child(2) > td:nth-child(5)")
