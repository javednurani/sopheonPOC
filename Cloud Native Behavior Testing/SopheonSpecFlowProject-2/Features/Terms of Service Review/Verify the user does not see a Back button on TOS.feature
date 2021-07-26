Feature: Verify the user does not see a Back button on TOS
    @TestCaseKey=CLOUD-T48
    Scenario: Verify the user does not see a Back button on TOS
        
        Given the user is on the TOS page
        
        When the user is on the TOS page the user will not have the option to click on a back button from the page
        
        Then user can still click on the back button on the browser and be taken to the Sign Up page from which they came
        
        When the user click on the back button on the browser
        
        Then the user is taken to a blank Login Screen