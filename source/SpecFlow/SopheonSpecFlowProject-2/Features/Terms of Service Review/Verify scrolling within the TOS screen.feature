Feature: Verify scrolling within the TOS screen
    @TestCaseKey=CLOUD-T50
    Scenario: Verify scrolling within the TOS screen
        
        Given the user is on the TOS screen
        
        When the user uses the scroll bar 
        
        Then the Terms of Service scrolls down the page