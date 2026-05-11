Feature: Generate a PDF for an assignment
  As a teacher
  I want to download an assignment as a fillable PDF
  So that students can fill it out offline and submit it back

  Background:
    Given an assignment titled "Math homework"

  Scenario: Each question has a fillable answer field
    Given an exercise with 3 questions is added
    When I generate the PDF
    Then the PDF has 3 fillable answer fields
    And every answer field name matches the pattern "q_{guid}_answer"

  Scenario: Metadata fields are present
    When I generate the PDF
    Then the PDF has a fillable field named "meta_studentName"
    And the PDF has a fillable field named "meta_date"
    And the PDF has a read-only field named "meta_assignmentId" with the assignment id as its value

  Scenario: PDF text never contains solution data
    Given an exercise with 1 question is added
    And the source exercise had a question solution "the answer is 42"
    When I generate the PDF
    Then the PDF text does not contain "the answer is 42"

  Scenario: A missing assignment yields a not-found error
    When I generate the PDF for an unknown assignment
    Then a not-found error is raised