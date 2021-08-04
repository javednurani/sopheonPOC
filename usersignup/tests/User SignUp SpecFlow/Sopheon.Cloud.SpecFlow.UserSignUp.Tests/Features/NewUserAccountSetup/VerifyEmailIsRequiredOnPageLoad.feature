Feature: Verify E-mail is Required on page load
    @TestCaseKey=CLOUD-T41
    Scenario: Verify E-mail is Required on page load
        
        Given the user is on the PL Account Setup Page
        
        When loads the page 
        
        Then the e-mail field is marked as required