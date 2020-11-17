const { isExportDeclaration } = require("typescript");

describe('Prelievi - List', () => {

    beforeEach(() => {    
      cy.login();
    })
  
    // afterEach(() => {
    //    cy.logout();
    // })

    // lista prelievi
    // it('List', () => {    
    
    //     cy.visit(Cypress.env('url') + '/prelievi');    

    //     cy.get('table > tbody').find('tr').should('have.length', 1);
  
    //     cy.get('#cy-data-inizio').type('01-01-2020');
    //     cy.get('#cy-data-fine').type('01-01-2021');
    //     cy.get('#cy-lotto').type('TT2909201612');

    //     cy.get('#cy-btn-search').click();
  
    //     cy.get('table > tbody').find('tr').should('not.have.length', 1);
        
    // })

    // gestione parametri hash
    it('Hash', () => {    
        
        cy.visit(Cypress.env('url') + '/prelievi');    

        cy.url().should('not.contains', 'IdDestinatario');

        cy.get('#cy-data-inizio').type('01-01-2020');
        cy.get('#cy-data-fine').type('01-01-2021');

        cy.get('#cy-lotto').type('1234');
        cy.select2_PickValue('#cy-destinatario');        

        cy.url().should('contains', 'IdDestinatario');

        cy.visit(Cypress.env('url'));    
        cy.go('back');

        cy.url().should('contains', 'IdDestinatario');

        cy.get('#cy-lotto').then((lotto) => {
          expect(lotto.val()).equal('1234');
        });

    })
  
  })
  