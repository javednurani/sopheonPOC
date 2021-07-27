Feature: Verify implicit acceptance
    @TestCaseKey=CLOUD-T46
    Scenario: Verify implicit acceptance
        
        Given the user is on the TOS page
        
        When the user clicks on Continue
        
        Then the user account is created