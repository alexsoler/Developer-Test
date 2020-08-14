Vue.component('input-select-currency', {
    props: {
        selectedCurrency: {
            type: String,
            default: 'USD'
        }
    },
    data: function () {
        return {
            currencies: ['EUR', 'USD', 'JPY', 'BGN', 'CZK', 'DKK', 'GBP', 'HUF', 'PLN', 'RON', 'SEK', 'CHF', 'ISK', 'NOK', 'HRK', 'RUB', 'TRY', 'AUD', 'BRL', 'CAD', 'CNY', 'HKD', 'IDR', 'ILS', 'INR', 'KRW', 'MXN', 'MYR', 'NZD', 'PHP', 'SGD', 'THB', 'ZAR'],
        }
    },
    methods: {
        selectCurrency: function(currency) {
            this.$emit('update-currency', currency)
        }
    },
    template: /*html*/ `
    <div class="input-group-prepend">
        <button class="btn btn-outline-secondary dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i :class="['currency-flag', 'currency-flag-' + selectedCurrency.toLowerCase()]"></i> {{ selectedCurrency }}
        </button>
        <div class="dropdown-menu">
            <button v-for="item in currencies" :key="item" class="dropdown-item" type="button" @click="selectCurrency(item)">
                <i :class="['currency-flag', 'currency-flag-' + item.toLowerCase()]"></i> {{ item }}
            </button>
        </div>
    </div>
    `
})

const app = new Vue({
    el: '#app',
    data: {
        currency1: {
            name: 'USD',
            value: 0
        },
        currency2: {
            name: 'EUR',
            value: 0
        }
    },
    methods: {
        convert: function(currencyFrom, currencyTo) {
            fetch(`https://api.exchangeratesapi.io/latest?base=${currencyFrom.name}&symbols=${currencyTo.name}`)
                .then(response => response.json())
                .then(data => currencyTo.value = data.rates[currencyTo.name] * currencyFrom.value)
        },
        updateCurrancy1: function(value) {
            this.currency1.name = value
            this.convert(this.currency1, this.currency2)
        },
        updateCurrancy2: function(value) {
            this.currency2.name = value
            this.convert(this.currency2, this.currency1)
        }
    }
})