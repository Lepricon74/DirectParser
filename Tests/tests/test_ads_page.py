import pytest
from pages.ads_page import AdsPage


@pytest.mark.basic_elements_presence
class TestPresenceInMainPage:
    LINK = 'http://89.108.88.254:84/'

    @pytest.mark.element_presence
    def test_icon_presence(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.should_be_main_icon()

    @pytest.mark.element_presence
    def test_table_presence(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.should_be_ads_table()

    @pytest.mark.element_presence
    def test_filter_presence(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.should_be_filter()

    @pytest.mark.element_presence
    def test_second_date_input_disabled(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.should_be_second_date_input_disabled()


class TestFilter:
    LINK = 'http://89.108.88.254:84/'

    @pytest.mark.filter
    def test_second_date_input_should_be_enabled_after_choose_first_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.should_be_second_date_input_enabled_after_choose_first_date()

    @pytest.mark.filter
    def test_descending_all_include_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_selected_filter('1')
        page.add_date_for_all_row()
        page.filter_descending_with_all_include_date()

    @pytest.mark.filter
    def test_without_filter_filter_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_date_for_several_row()
        page.without_filter_with_filter_date()

    @pytest.mark.filter
    def test_ascending_all_include_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_selected_filter('2')
        page.add_date_for_all_row()
        page.filter_ascending_with_all_include_date()

    @pytest.mark.filter
    def test_ascending_without_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_selected_filter('2')
        page.filter_ascending_without_date()

    @pytest.mark.filter
    def test_descending_filter_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_selected_filter('1')
        page.add_date_for_several_row()
        page.filter_descending_filter_date()

    @pytest.mark.filter
    def test_without_filter_all_include_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_date_for_all_row()
        page.without_filter_all_include_date()

    @pytest.mark.filter
    def test_descending_without_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_selected_filter('1')
        page.filter_descending_without_date()

    @pytest.mark.filter
    def test_without_filter_without_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.without_filter_without_date()

    @pytest.mark.filter
    def test_ascending_filter_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_selected_filter('2')
        page.add_date_for_several_row()
        page.filter_ascending_filter_date()

    @pytest.mark.filter
    def test_without_filter_filter_date(self, browser):
        page = AdsPage(browser, self.LINK)
        page.open()
        page.add_date_for_several_row()
        page.without_filter_filter_date()