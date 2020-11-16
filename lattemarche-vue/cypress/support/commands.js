// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************

// login
Cypress.Commands.add("login", (email, password) => { 

    cy.visit(Cypress.env('url') + '/account/login');

    cy.get('#Username').type(Cypress.env('username'));
    cy.get('#Password').type(Cypress.env('password'));

    cy.get('#login').click();

 })

// logout
 Cypress.Commands.add("logout", (email, password) => { 

    cy.get('.cy-username').click();
    cy.get('#cy-btn-logout').click();

 })

// select2 setFirstValue
Cypress.Commands.add("select2_PickValue", (id) => {

   cy.get(id).find('option').its('length').should('be.gte', 0);   
   cy.get(id + ' + .select2').find('.select2-selection').click();
   cy.get('.select2-results__option').first().click();

})

// select2 searchValue
Cypress.Commands.add("select2_TypeValue", (id, api, value) => {

   cy.get(id + ' + .select2');
   cy.get(id + ' + .select2').find('.select2-selection').click();
   
   cy.route(api).as('search');
   cy.get('.select2-search__field').first().type(value);   

   cy.wait('@search');
   cy.get('.select2-results__option--highlighted').should('be.visible');
   cy.get('.select2-search__field').first().type('{enter}');

})

// close modal dialog+
Cypress.Commands.add("closeDialog", (id) => {

   cy.get(id + " .cy-btn-ok").should('be.visible');       
   cy.wait(500);   
   cy.get(id + " .cy-btn-ok").click();
   cy.get(id + " .cy-btn-ok").should('not.visible'); 
})

 // -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This is will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })
