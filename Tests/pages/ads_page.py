from .base_page import BasePage
from .locators import AdsPageLocators


class AdsPage(BasePage):

    def should_be_ads_table(self):
        assert self.is_not_element_present(*AdsPageLocators.ADS_TABLE) is False, "Ads Table is not presented,"
