import { isProd } from './settings/environmentSettings';

const pendoKey = isProd ? '^PendoApiKey^' : '';

(function (apiKey) {
    (function (p, e, n, d, o) {
        let v;
        let w;
        let x;
        let y;
        let z;
        o = p[d] = p[d] || {};
        o._q = o._q || [];
        v = ['initialize', 'identify', 'updateOptions', 'pageLoad', 'track'];
        for (w = 0, x = v.length; w < x; ++w) {
            (function (m) {
                o[m] =
                    o[m] ||
                    function () {
                        o._q[m === v[0] ? 'unshift' : 'push']([m].concat([].slice.call(arguments, 0)));
                    };
            })(v[w]);
        }
        try {
            if (!apiKey) {
                return;
            }
            y = e.createElement(n);
            y.async = !0;
            y.src = `https://cdn.pendo.io/agent/static/${apiKey}/pendo.js`;
            z = e.getElementsByTagName(n)[0];
            z.parentNode.insertBefore(y, z);
            // eslint-disable-next-line no-empty
        } catch { }
    })(window, document, 'script', 'pendo');
})(pendoKey);

export const { pendo } = window;
