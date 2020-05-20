/// <reference types="Cypress" />

context('Actions', () => {
  beforeEach(() => {
    cy.visit(Cypress.env('url'));
  })

  it('Login', () => {
    
    cy.get('#btn-login').click()          
    cy.url().should('include', '/account/login')

    cy.get('#Username').type(Cypress.env('username'))
    cy.get('#Password').type(Cypress.env('password'))

    // cy.get('#login').click()        

    // cy.get('h1').should('contain', 'Home')

  })

})
