Feature: Generate a PDF for an assignment
  As a teacher
  I want to download an assignment as a fillable PDF
  So that students can fill it out offline and submit it back

  Background:
    Given an assignment titled "Math homework"

  Scenario: PDF text never contains solution data
    Given an exercise with 1 question is added
    And the source exercise had a question solution "the answer is 42"
    When I generate the PDF
    Then the PDF text does not contain "the answer is 42"

  Scenario: A missing assignment yields a not-found error
    When I generate the PDF for an unknown assignment
    Then a not-found error is raised
