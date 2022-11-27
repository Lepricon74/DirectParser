import pytest
from pages.ads_page import AdsPage


@pytest.mark.basic_elements_presence
class TestPresenceInMainPage:
    LINK = 'http://89.108.88.254:84/'

    def test_icon_presence(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.should_be_main_icon()

    def test_table_presence(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.should_be_ads_table()