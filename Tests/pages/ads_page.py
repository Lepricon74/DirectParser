from .base_page import BasePage
from .locators import AdsPageLocators
from selenium.webdriver.support.ui import Select
from selenium.webdriver.common.by import By


class AdsPage(BasePage):

    def should_be_ads_table(self):
        assert self.is_not_element_present(*AdsPageLocators.ADS_TABLE) is False, "Ads Table is not presented,"

    def should_be_filter(self):
        assert self.is_element_present(*AdsPageLocators.FILTER) is True, "Filter is not presented"

    def should_be_second_date_input_disabled(self):
        second_date_input = self.browser.find_element(*AdsPageLocators.END_DATE)
        assert second_date_input.is_enabled() is False, "Second date input is not disabled"

    def should_be_second_date_input_enabled_after_choose_first_date(self):
        first_date_input = self.browser.find_element(*AdsPageLocators.START_DATE)
        first_date_input.send_keys("2022-12-15T00:07")
        second_date_input = self.browser.find_element(*AdsPageLocators.END_DATE)
        assert second_date_input.is_enabled() is True, "Second date input is disabled"

    def add_date_for_all_row(self):
        start_date = self.browser.find_element(*AdsPageLocators.START_DATE)
        end_date = self.browser.find_element(*AdsPageLocators.END_DATE)
        start_date.send_keys("16-04-00202200:00")
        end_date.send_keys("30-04-00202200:00")

    def add_date_for_several_row(self):
        start_date = self.browser.find_element(*AdsPageLocators.START_DATE)
        end_date = self.browser.find_element(*AdsPageLocators.END_DATE)
        start_date.send_keys("26-04-00202200:00")
        end_date.send_keys("30-04-00202200:00")

    def add_selected_filter(self, value):
        filter_select = Select(self.browser.find_element(*AdsPageLocators.FILTER_SELECT))
        filter_select.select_by_value(value)

    def find_button_and_rows(self):
        button = self.browser.find_element(*AdsPageLocators.FILTER_BUTTON)
        button.click()
        first_tbody = self.browser.find_element(*AdsPageLocators.FIRST_TBODY)
        rows = first_tbody.find_elements(*AdsPageLocators.ROWS)
        first_row_date_value = self.browser.find_element(*AdsPageLocators.FIRST_ROW_DATE).text
        second_row_date_value = self.browser.find_element(*AdsPageLocators.SECOND_ROW_DATE).text
        return {'rows': rows, 'first_date_value': first_row_date_value, 'second__date_value' : second_row_date_value}


    def filter_descending_with_all_include_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 4, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Sat Apr 30 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Sat Apr 30 2022', "Second Data is incorrect"

    def filter_ascending_with_all_include_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 4, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Mon Apr 25 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Mon Apr 25 2022', "Second Data is incorrect"

    def without_filter_with_filter_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 2, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Sat Apr 30 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Sat Apr 30 2022', "Second Data is incorrect"

    def filter_ascending_without_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 6, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Mon Apr 25 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Mon Apr 25 2022', "Second Data is incorrect"

    def filter_descending_filter_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 2, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Sat Apr 30 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Sat Apr 30 2022', "Second Data is incorrect"

    def without_filter_all_include_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 4, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Sat Apr 30 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Mon Apr 25 2022', "Second Data is incorrect"

    def filter_descending_without_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 6, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Sat Apr 30 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Sat Apr 30 2022', "Second Data is incorrect"

    def without_filter_without_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 6, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Sat Apr 30 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'В объявлении нет акций', "Second Data is incorrect"

    def filter_ascending_filter_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 2, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Sat Apr 30 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Sat Apr 30 2022', "Second Data is incorrect"

    def without_filter_filter_date(self):
        data_object = self.find_button_and_rows()
        assert len(data_object['rows']) == 2, "Count of rows is incorrect"
        assert data_object['first_date_value'] == 'Sat Apr 30 2022', "First Data is incorrect"
        assert data_object['second__date_value'] == 'Sat Apr 30 2022', "Second Data is incorrect"